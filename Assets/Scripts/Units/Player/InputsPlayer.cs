using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(AimingComponent))]
public class InputsPlayer : ControlInputsBase
{
    private PlayerControls _controls;

    private IsoCamAdjust _adj;
    public TargetUnitPanel TargetPanel { get; set; }
    private AimingComponent _aim;

    public ComboController GetComboController => _comboCtrl;

    public event SimpleEventsHandler<EquipItemType> ChangeLayerEvent;
    // only one gain per swing, changed by unit
    public bool ComboGained { get; set; } = false;

    public SimpleEventsHandler<GameMenuType> MenuToggleRequest;



    private void OnDisable()
    {
        PlayerBind(false);
    }

    public override void BindControllers(bool isEnable)
    {
        _controls = new PlayerControls();
        base.BindControllers(isEnable);
        PlayerBind();
    }

    private void Start()
    {
        transform.LookAt(transform.forward);
    }

    private void PlayerBind(bool enable = true)
    {
        if (enable)
        {
            _controls.Game.Enable();
            _adj = new IsoCamAdjust();

            _aim = GetComponent<AimingComponent>();

            _controls.Game.Dash.performed += Dash_performed;
            _controls.Game.SkillE.performed += SkillE_performed;
            _controls.Game.SkillQ.performed += SkillQ_performed;
            _controls.Game.SkillR.performed += SkillR_performed;
            _controls.Game.MainAttack.performed += MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed += RangedAttack_performed;
            _controls.Game.Pause.performed += (t) => MenuToggleRequest?.Invoke(GameMenuType.Pause);
            _controls.Game.Items.performed += (t) => MenuToggleRequest?.Invoke(GameMenuType.Items);



            _weaponCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;

            _skillCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += _weaponCtrl.SwitchModels;

        }
        else
        {
            _controls.Game.Disable();

            _controls.Game.Dash.performed -= Dash_performed;
            _controls.Game.SkillE.performed -= SkillE_performed;
            _controls.Game.SkillQ.performed -= SkillQ_performed;
            _controls.Game.SkillR.performed -= SkillR_performed;
            _controls.Game.MainAttack.performed -= MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed -= RangedAttack_performed;
            _controls.Game.Pause.performed -= (t) => MenuToggleRequest?.Invoke(GameMenuType.Pause);
            _controls.Game.Items.performed -= (t) => MenuToggleRequest?.Invoke(GameMenuType.Items);


            _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= _weaponCtrl.SwitchModels;

        }
    }


    private void SwitchAnimatorLayer(EquipItemType type)
    {
        if (IsControlsBusy) return;
        if (_weaponCtrl.CurrentWeaponType != type)
        {
            ChangeLayerEvent?.Invoke(type);
        }
    }


    #region controller checks

    protected void RangedAttack_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_weaponCtrl.UseWeaponCheck(EquipItemType.RangedWeap, out string text))
            CombatActionSuccessCallback(CombatActionType.Ranged);
        else Debug.Log(text);

    }
    protected void MeleeAttack_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_weaponCtrl.UseWeaponCheck(EquipItemType.MeleeWeap, out string text))
            CombatActionSuccessCallback(CombatActionType.Melee);
        else Debug.Log(text);
    }
    protected void SkillR_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.ShieldSpecialR, out var c))
        {
            if (_comboCtrl.UseCombo(c))
                CombatActionSuccessCallback(CombatActionType.ShieldSpecialR);
        }
    }
    protected void SkillQ_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.MeleeSpecialQ, out var c))
        {
            if (_comboCtrl.UseCombo(c))
                CombatActionSuccessCallback(CombatActionType.MeleeSpecialQ);
        }
    }
    protected void SkillE_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.RangedSpecialE, out var c))
        {
            if (_comboCtrl.UseCombo(c))
                CombatActionSuccessCallback(CombatActionType.RangedSpecialE);
        }
    }
    private void Dash_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_dodgeCtrl.IsDodgePossibleCheck())
        {
            CombatActionSuccessCallback(CombatActionType.Dodge);
        }
    }
    #endregion

    private void LateUpdate()
    {
        if (IsControlsBusy) return;
        CalculateMovement();
        Aiming();
    }

    private void CalculateMovement()
    {

        if (IsControlsBusy) return;
        if (_controls == null) return;
        var input = _controls.Game.WASD.ReadValue<Vector2>();
        Vector3 AD = _adj.Isoright * input.x;
        Vector3 WS = _adj.Isoforward * input.y;
        velocityVector = AD + WS;
    }
    private void Aiming()
    {
        if (_aim == null) return;
        LerpRotateToTarget(_aim.GetLookPoint);


        if (_aim.GetItem is NPCUnit) TargetPanel.AssignItem(_aim.GetItem as NPCUnit,true);
        // massive TODO here
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + MoveDirection);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward + transform.position);

    }

    public override UnitType GetUnitType()
    {
        return UnitType.Player;
    }
}