using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(AimingComponent))]
public class InputsPlayer : ControlInputsBase
{
    private PlayerControls _controls;

    [SerializeField] private DodgeController _dodgeCtrl;
    [SerializeField] private ShieldController _shieldCtrl;
    [SerializeField] private ComboController _comboCtrl;

    private IsoCamAdjust _adj;
    public TargetUnitPanel TargetPanel { get; set; }
    private AimingComponent _aim;

    public DodgeController GetDodgeController => _dodgeCtrl;
    public ShieldController GetShieldController => _shieldCtrl;
    public ComboController GetComboController => _comboCtrl;

    public event SimpleEventsHandler<WeaponType> ChangeLayerEvent;
    // only one gain per swing, changed by unit
    public bool ComboGained { get; set; } = false;

    public SimpleEventsHandler<GameMenuType> MenuToggleRequest;



    private void OnDisable()
    {
        PlayerBind(false);
    }

    public override void BindControllers(bool isEnable)
    {
        base.BindControllers(isEnable);
        _controls = new PlayerControls();
        _shieldCtrl = new ShieldController(_statsCtrl.GetUnitID);
        _dodgeCtrl = new DodgeController(_statsCtrl.GetUnitID);
        _comboCtrl = new ComboController(_statsCtrl.GetUnitID);
    }

    private void Start()
    {
        PlayerBind();
        transform.LookAt(transform.forward);
    }

    private void PlayerBind(bool enable = true)
    {
        if (enable)
        {
            _controls.Game.Enable();
            Cursor.visible = false;


            //_playerWeaponCtrl = _weaponCtrl as PlayerWeaponController;

            _adj = new IsoCamAdjust();
            _handler.RegisterUnitForStatUpdates(_dodgeCtrl);
            _handler.RegisterUnitForStatUpdates(_shieldCtrl);
            _handler.RegisterUnitForStatUpdates(_comboCtrl);


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

            _weaponCtrl.TargetHitByWeaponEvent += HandleComboGain;

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
            _controls.Game.Pause.performed -= (t) => MenuToggleRequest?.Invoke(GameMenuType.Pause);
            _controls.Game.Items.performed -= (t) => MenuToggleRequest?.Invoke(GameMenuType.Items);


            _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= _weaponCtrl.SwitchModels;
            _weaponCtrl.TargetHitByWeaponEvent -= HandleComboGain;

        }
    }

    private void HandleComboGain(float value)
    {
        if (ComboGained) return;
        else
        {
            _comboCtrl.AddCombo(value);
            ComboGained = true;
        }
    }
    private void SwitchAnimatorLayer(WeaponType type)
    {
        if (_weaponCtrl.GetCurrentWeaponType != type)
        {
            ChangeLayerEvent?.Invoke(type);
        }
    }


    #region controller checks

    protected void RangedAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Ranged, out string text))
            CombatActionSuccessCallback(CombatActionType.Ranged);
        else Debug.Log(text);

    }
    protected void MeleeAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee, out string text))
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

    private void Update()
    {
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