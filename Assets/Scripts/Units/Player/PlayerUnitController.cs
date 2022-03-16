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
    public event SimpleEventsHandler<WeaponType> ChangeLayerEvent;
    private void DoSwitchLayer(WeaponType type) => ChangeLayerEvent?.Invoke(type);

    private void OnDisable()
    {
        _controls.Game.Disable();
    }
    public Vector3 CurrentCursorPosition { get; private set; }
    private IsoCamAdjust _adj;
    [SerializeField, Tooltip("How high should the aiming be done")] private float _targetOffsetY = 1f;

    [SerializeField] private DodgeController _dodgeCtrl;

    protected override void Awake()
    {
        base.Awake();
        _controls = new PlayerControls();
        _controls.Game.Enable();


        _adj = new IsoCamAdjust();

        _weaponCtrl = _baseWeap as PlayerWeaponController;
        _weaponCtrl.WeaponSwitchEvent += DoSwitchLayer;

        _dodgeCtrl.Initialize(this);
        _handler.RegisterUnitForStatUpdates(_dodgeCtrl);

        _controls.Game.Dash.performed += Dash_performed;
        _controls.Game.SkillE.performed += SkillE_performed;
        _controls.Game.SkillQ.performed += SkillQ_performed;
        _controls.Game.SkillR.performed += SkillR_performed;
        _controls.Game.MainAttack.performed += MeleeAttack_performed;
        _controls.Game.SpecialAttack.performed += RangedAttack_performed;
    }

    // call these after all checks are done
    public SimpleEventsHandler PlayerMeleeAttackSuccessEvent;
    public SimpleEventsHandler PlayerRangedAttackSuccessEvent;
    public SimpleEventsHandler PlayerQSuccessEvent;
    public SimpleEventsHandler PlayerESuccessEvent;
    public SimpleEventsHandler PlayerRSuccessEvent;
    public SimpleEventsHandler PlayerDashSuccessEvent;

    #region logic and checks
    private void RangedAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Ranged))
        PlayerRangedAttackSuccessEvent?.Invoke();
    }
    private void MeleeAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee))
            PlayerMeleeAttackSuccessEvent?.Invoke();
    }
    private void SkillR_performed(CallbackContext obj)
    {
        PlayerRSuccessEvent?.Invoke();
    }
    private void SkillQ_performed(CallbackContext obj)
    {
        PlayerQSuccessEvent?.Invoke();
    }
    private void SkillE_performed(CallbackContext obj)
    {
        PlayerESuccessEvent?.Invoke();
    }
    private void Dash_performed(CallbackContext obj)
    {
        if (_dodgeCtrl.IsDodgePossibleCheck())
        PlayerDashSuccessEvent?.Invoke();
    }

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
            if (!hit.collider.CompareTag("Ground")) return;
            CurrentCursorPosition = hit.point;
            transform.LookAt(CurrentCursorPosition);
        }
    }
}

