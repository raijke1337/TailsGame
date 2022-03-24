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
    [Inject]
    protected StatsUpdatesHandler _handler;

    private PlayerControls _controls;

    [SerializeField] private PlayerWeaponController _weaponCtrl;
    [SerializeField] private DodgeController _dodgeCtrl;
    [SerializeField] private SkillsController _skillCtrl;

    public DodgeController GetDodgeController => _dodgeCtrl;
    public BaseWeaponController GetWeaponController => _weaponCtrl;
    public SkillsController GetSkillsController => _skillCtrl;

    // call these after all checks are done
    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    public event SimpleEventsHandler DodgeCompletedAnimatingEvent;
    public event SimpleEventsHandler<WeaponType> ChangeLayerEvent;
    public event SimpleEventsHandler<BaseUnit> TargetLockedEvent;
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
        PlayerBind();
    }

    private void PlayerBind(bool enable = true)
    {
        if (enable)
        {
            _controls.Game.Enable();
            Cursor.visible = false;

            _adj = new IsoCamAdjust();

            _handler.RegisterUnitForStatUpdates(_dodgeCtrl);
            _handler.RegisterUnitForStatUpdates(_skillCtrl);

            _controls.Game.Dash.performed += Dash_performed;
            _controls.Game.SkillE.performed += SkillE_performed;
            _controls.Game.SkillQ.performed += SkillQ_performed;
            _controls.Game.SkillR.performed += SkillR_performed;
            _controls.Game.MainAttack.performed += MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed += RangedAttack_performed;

            _weaponCtrl = _baseWeap as PlayerWeaponController;
            _weaponCtrl.WeaponSwitchEvent += DoSwitchLayer;

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

            _weaponCtrl.WeaponSwitchEvent -= DoSwitchLayer;
        }

    }

    #region controller checks

    // todo make an enum with action types and unified calls
    private void RangedAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Ranged))
            CombatActionSuccessEvent?.Invoke(CombatActionType.Ranged);
    }
    private void MeleeAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee))
            CombatActionSuccessEvent?.Invoke(CombatActionType.Melee);
    }
    private void SkillR_performed(CallbackContext obj)
    {
        if (_skillCtrl.RequestSkill(CombatActionType.ShieldSpecialR))
        CombatActionSuccessEvent?.Invoke(CombatActionType.ShieldSpecialR);
        Debug.Log(obj.action);
    }
    private void SkillQ_performed(CallbackContext obj)
    {
        if (_skillCtrl.RequestSkill(CombatActionType.MeleeSpecialQ))
            CombatActionSuccessEvent?.Invoke(CombatActionType.MeleeSpecialQ);
        Debug.Log(obj.action);
    }
    private void SkillE_performed(CallbackContext obj)
    {
        if (_skillCtrl.RequestSkill(CombatActionType.RangedSpecialE))
            CombatActionSuccessEvent?.Invoke(CombatActionType.RangedSpecialE);
        Debug.Log(obj.action);

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
        CalculateAimPoint();
        _aim.SetLookTarget(lookTarget);
        transform.LookAt(lookTarget);

        CalculateMovement();
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
    #endregion
}