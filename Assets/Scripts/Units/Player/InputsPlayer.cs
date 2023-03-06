using System.Reflection;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(AimingComponent))]
public class InputsPlayer : ControlInputsBase
{
    private PlayerControls _controls;

    private IsoCamAdjust _adj;
    private AimingComponent _aim;


    public ComboController GetComboController => _comboCtrl;
    public AimingComponent GetAimingComponent => _aim;

    public event SimpleEventsHandler<EquipItemType> ChangeLayerEvent;
    // only one gain per swing, changed by unit
    public bool ComboGained { get; set; } = false;

    #region managedctrl

    public override void StartController()
    {
        base.StartController();

        if (GameManager.Instance.GetCurrentLevelData.Type != LevelType.Game) return;

        _controls = new PlayerControls();
        _controls.Game.Enable();
        _aim = GetComponent<AimingComponent>();

        _controls.Game.Dash.performed += Dash_performed;
        _controls.Game.SkillE.performed += SkillE_performed;
        _controls.Game.SkillQ.performed += SkillQ_performed;
        _controls.Game.SkillR.performed += SkillR_performed;
        _controls.Game.MainAttack.performed += MeleeAttack_performed;
        _controls.Game.SpecialAttack.performed += RangedAttack_performed;

        _weaponCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
        _skillCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
        _skillCtrl.SwitchAnimationLayersEvent += _weaponCtrl.SwitchModels;
        transform.LookAt(transform.forward);
    }
    public override void UpdateController(float delta)
    {
        base.UpdateController(delta);
        if (IsControlsBusy) return; // set during dodges and attack motions
        _adj ??= new IsoCamAdjust();
        CalculateMovement();
        Aiming();
    }

    public override void StopController()
    {
        base.StopController();
        _controls.Game.Disable();

        _controls.Game.Dash.performed -= Dash_performed;
        _controls.Game.SkillE.performed -= SkillE_performed;
        _controls.Game.SkillQ.performed -= SkillQ_performed;
        _controls.Game.SkillR.performed -= SkillR_performed;
        _controls.Game.MainAttack.performed -= MeleeAttack_performed;
        _controls.Game.SpecialAttack.performed -= RangedAttack_performed;


        _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
        _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
        _skillCtrl.SwitchAnimationLayersEvent -= _weaponCtrl.SwitchModels;
    }

    #endregion


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
        if (velocityVector == Vector3.zero) return; //can't dash from standing
        if (_dodgeCtrl.IsDodgePossibleCheck())
        {
            CombatActionSuccessCallback(CombatActionType.Dodge);
        }
    }
    #endregion


    private void CalculateMovement()
    {

        if (IsControlsBusy) return;
        Vector2 input;
        if (InputDirectionOverride != Vector3.zero)
        {
            input = InputDirectionOverride;
        }
        else
        {
            if (_controls == null) return; //shouldn happen
            input = _controls.Game.WASD.ReadValue<Vector2>();
        }
        Vector3 AD = _adj.Isoright * input.x;
        Vector3 WS = _adj.Isoforward * input.y;
        velocityVector = AD + WS;
    }
    private void Aiming()
    {
        if (_aim == null || InputDirectionOverride != Vector3.zero) return;
        LerpRotateToTarget(_aim.GetLookPoint, lastDelta);
    }


    protected override void LerpRotateToTarget(Vector3 looktarget, float delta) // here we also apply turning animations
    {
        base.LerpRotateToTarget(looktarget, delta);
        // TODO Detect rotation
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