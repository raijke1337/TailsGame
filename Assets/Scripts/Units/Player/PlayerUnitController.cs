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
    private PlayerWeaponController _weaponCtrl;
    [SerializeField] private DodgeController _dodgeCtrl;

    public DodgeController GetDodgeController => _dodgeCtrl;
    public BaseWeaponController GetWeaponController => _weaponCtrl;

    // call these after all checks are done
    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    public event SimpleEventsHandler DodgeCompletedAnimatingEvent;
    public event SimpleEventsHandler<WeaponType> ChangeLayerEvent;

    public event SimpleEventsHandler<IStatsAvailable> TargetLockedEvent;

    private void DoSwitchLayer(WeaponType type) => ChangeLayerEvent?.Invoke(type);

    private void OnDisable()
    {
        _controls.Game.Disable();
    }
    public Vector3 CurrentCursorPosition { get; private set; }
    private IsoCamAdjust _adj;
    [SerializeField, Tooltip("How high should the aiming be done")] private float _targetOffsetY = 1f;



    protected override void Awake()
    {
        base.Awake();
        _controls = new PlayerControls();
        _controls.Game.Enable();


        _adj = new IsoCamAdjust();

        _weaponCtrl = _baseWeap as PlayerWeaponController;
        _weaponCtrl.WeaponSwitchEvent += DoSwitchLayer;

        _handler.RegisterUnitForStatUpdates(_dodgeCtrl);

        _controls.Game.Dash.performed += Dash_performed;
        _controls.Game.SkillE.performed += SkillE_performed;
        _controls.Game.SkillQ.performed += SkillQ_performed;
        _controls.Game.SkillR.performed += SkillR_performed;
        _controls.Game.MainAttack.performed += MeleeAttack_performed;
        _controls.Game.SpecialAttack.performed += RangedAttack_performed;
    }


    #region logic and checks

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
        CombatActionSuccessEvent?.Invoke(CombatActionType.ShieldSpecialR);
    }
    private void SkillQ_performed(CallbackContext obj)
    {
        CombatActionSuccessEvent?.Invoke(CombatActionType.MeleeSpecialQ);
    }
    private void SkillE_performed(CallbackContext obj)
    {
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
    //private void AddDodgeForce()
    //{
    //    GetUnit.GetRigidBody.AddForce(10* _movement * _dodgeCtrl.GetDodgeStats()[DodgeStatType.Range].GetCurrent(), ForceMode.Impulse);
    //    DodgeCompletedAnimatingEvent?.Invoke();
    //}
    // todo doesnt work


    #endregion

    private void Update()
    {
        CalculateAimPoint();
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        var input = _controls.Game.WASD.ReadValue<Vector2>();
        Vector3 AD = _adj.Isoright * input.x;
        Vector3 WS = _adj.Isoforward * input.y;
        _movement = AD + WS;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(CurrentCursorPosition, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_movement,0.1f);
    }
#endif

    // todo
    private void CalculateAimPoint()
    {
        // todo fix rotations
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit)) return;
        else
        {
            if (!(hit.collider.CompareTag("Ground")) &&
                (hit.collider.gameObject.GetComponent<IStatsAvailable>() == null) ) return;
            CurrentCursorPosition = hit.point;
            transform.LookAt(CurrentCursorPosition);
            if (hit.collider.gameObject.GetComponent<IStatsAvailable>() != null)
            {
                var tgt = hit.collider.gameObject.GetComponent<IStatsAvailable>();
                TargetLockedEvent?.Invoke(tgt);
            }    
        }
    }
    // doesnt look too good
    private IEnumerator DoDodgeMovement()
    {
        Vector3 start = transform.position;
        Vector3 finish = transform.position +
                (_movement * _dodgeCtrl.GetDodgeStats()[DodgeStatType.Range].GetCurrent());

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
}

