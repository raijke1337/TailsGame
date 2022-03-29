using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Zenject;


public class PlayerUnitController : BaseUnitController
{
    private PlayerControls _controls;

    [SerializeField] private PlayerWeaponController _playerWeaponCtrl;
    [SerializeField] private DodgeController _dodgeCtrl;
    [SerializeField] private SkillsController _skillCtrl;
    [SerializeField] private string _shieldSkillID; // todo?

    public DodgeController GetDodgeController => _dodgeCtrl;

    public SkillsController GetSkillsController => _skillCtrl;

    // call these after all checks are done
    public event SimpleEventsHandler DodgeCompletedAnimatingEvent;
    public event SimpleEventsHandler<WeaponType> ChangeLayerEvent;
    public event SimpleEventsHandler<BaseUnit> TargetLockedEvent;
    public override event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    private void DoSwitchLayer(WeaponType type) => ChangeLayerEvent?.Invoke(type);

    private void OnDisable()
    {
        PlayerBind(false);
    }
    private IsoCamAdjust _adj;

    private CrosshairScript _aim;
    private Ray ray;
    private Vector3 lookTarget;
    public Vector3 GetLookTarget() => lookTarget;

    protected override void Awake()
    {
        base.Awake();
        _controls = new PlayerControls();
        // unique for player
    }

    private void Start()
    {
        PlayerBind();
    }

    private void PlayerBind(bool enable = true)
    {
        if (enable)
        {
            _controls.Game.Enable();
            Cursor.visible = false;

            _playerWeaponCtrl = _weaponCtrl as PlayerWeaponController;

            _adj = new IsoCamAdjust();

            var skills = _playerWeaponCtrl.GetSkillIDs(); // run later 

            skills.Add(_shieldSkillID);
            _skillCtrl = new SkillsController(skills);


            _handler.RegisterUnitForStatUpdates(_dodgeCtrl);
            _handler.RegisterUnitForStatUpdates(_skillCtrl);

            _controls.Game.Dash.performed += Dash_performed;
            _controls.Game.SkillE.performed += SkillE_performed;
            _controls.Game.SkillQ.performed += SkillQ_performed;
            _controls.Game.SkillR.performed += SkillR_performed;
            _controls.Game.MainAttack.performed += MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed += RangedAttack_performed;

            _playerWeaponCtrl.WeaponSwitchEvent += DoSwitchLayer;

            _aim = GetComponentInChildren<CrosshairScript>();
            _aim.transform.parent = null;
        }
        else
        {
            _controls.Game.Disable();
            Cursor.visible = true;

            _controls.Game.Dash.performed -= Dash_performed;
            _controls.Game.SkillE.performed -= SkillE_performed;
            _controls.Game.SkillQ.performed -= SkillQ_performed;
            _controls.Game.SkillR.performed -= SkillR_performed;
            _controls.Game.MainAttack.performed -= MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed -= RangedAttack_performed;

            _playerWeaponCtrl.WeaponSwitchEvent -= DoSwitchLayer;
        }

    }

    #region controller checks

    protected override void RangedAttack_performed(CallbackContext obj)
    {
        if (_playerWeaponCtrl.UseWeaponCheck(WeaponType.Ranged))
            CombatActionSuccessEvent?.Invoke(CombatActionType.Ranged);
    }
    protected override void MeleeAttack_performed(CallbackContext obj)
    {
        if (_playerWeaponCtrl.UseWeaponCheck(WeaponType.Melee))
            CombatActionSuccessEvent?.Invoke(CombatActionType.Melee);
    }
    private void SkillR_performed(CallbackContext obj)
    {
        if (_skillCtrl.RequestSkill(CombatActionType.ShieldSpecialR))
        CombatActionSuccessEvent?.Invoke(CombatActionType.ShieldSpecialR);

    }
    private void SkillQ_performed(CallbackContext obj)
    {
        if (_skillCtrl.RequestSkill(CombatActionType.MeleeSpecialQ))
            CombatActionSuccessEvent?.Invoke(CombatActionType.MeleeSpecialQ);
    }
    private void SkillE_performed(CallbackContext obj)
    {
        if (_skillCtrl.RequestSkill(CombatActionType.RangedSpecialE))
            CombatActionSuccessEvent?.Invoke(CombatActionType.RangedSpecialE);
    }
    private void Dash_performed(CallbackContext obj)
    {
        if (_movement == Vector3.zero) return;
        if (_dodgeCtrl.IsDodgePossibleCheck())
        {
            CombatActionSuccessEvent?.Invoke(CombatActionType.Dodge);
            StartCoroutine(DoDodgeMovement());
            //AddDodgeForce();
        }
    }
    #endregion

    private void Update()
    {
        CalculateMovement();
        CalculateAimPoint();
        if (lookTarget == null) return;

        _aim.SetLookTarget(lookTarget);
        transform.LookAt(lookTarget);
    }

    private void CalculateMovement()
    {
        var input = _controls.Game.WASD.ReadValue<Vector2>();
        Vector3 AD = _adj.Isoright * input.x;
        Vector3 WS = _adj.Isoforward * input.y;
        _movement = AD + WS;
    }

    #region aiming
    private void CalculateAimPoint()
    {
        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit)) return;

        // todo use "interactible" and "interactor" classes here 
        if (hit.collider.CompareTag("Ground"))
        {
            var loc = hit.point;
            lookTarget = new Vector3(loc.x, 0, loc.z);
        }
        if (hit.collider.CompareTag("Enemy"))
        {
            var loc = hit.transform.position;
            lookTarget = new Vector3(loc.x, 0, loc.z);
            TargetLockedEvent?.Invoke(hit.collider.gameObject.GetComponent<BaseUnit>());
        }
    }

    #endregion

    #region dodges
    private IEnumerator DoDodgeMovement()
    {
        // todo clean up logic here

        Vector3 start = transform.position;
        //Vector3 finish = transform.position + (_movement * _dodgeCtrl.GetDodgeStats()[DodgeStatType.Range].GetCurrent());
        // dodge in direction of running
        // need - dodge in direction of mouse

        Vector3 finish = (transform.forward * _dodgeCtrl.GetDodgeStats()[DodgeStatType.Range].GetCurrent()) 
            + transform.position;

        float time = _dodgeCtrl.GetDodgeStats()[DodgeStatType.Duration].GetCurrent();
        float currenttime = 0;
        while (currenttime < time)
        {
            transform.position = Vector3.LerpUnclamped(start, finish, currenttime);
            currenttime += Time.deltaTime;
            yield return null;
        }
        DodgeCompletedAnimatingEvent?.Invoke();
        yield return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _movement);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward + transform.position);
    }

    protected override void UnitDiedAction(BaseUnit unit)
    {
        Debug.LogError("YOU DIED");
    }
    #endregion
}