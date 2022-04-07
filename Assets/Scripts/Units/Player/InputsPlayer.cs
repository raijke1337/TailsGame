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


public class InputsPlayer : ControlInputsBase
{
    private PlayerControls _controls;

    [SerializeField] private PlayerWeaponController _playerWeaponCtrl;
    [SerializeField] private DodgeController _dodgeCtrl;
    [SerializeField] private SkillsController _skillCtrl;
    [SerializeField] private ShieldController _shieldCtrl;

    private SelectorComponent _selector;


    [SerializeField] private string _shieldSkillID; // todo?

    public DodgeController GetDodgeController => _dodgeCtrl;
    public SkillsController GetSkillsController => _skillCtrl;
    public ShieldController GetShieldController => _shieldCtrl;


    public event SimpleEventsHandler<WeaponType> ChangeLayerEvent;
    public override event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;

    private void DoSwitchLayer(WeaponType type) => ChangeLayerEvent?.Invoke(type);

    private void OnDisable()
    {
        PlayerBind(false);
    }
    private IsoCamAdjust _adj;

    [SerializeField] private CrosshairScript _aim;

    private Vector3 lookTarget;
    public Vector3 GetLookTarget() => lookTarget;

    protected override void OnEnable()
    {
        base.OnEnable();
        _controls = new PlayerControls();
        // unique for player
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

            _selector = GetComponent<SelectorComponent>();

            _playerWeaponCtrl = _weaponCtrl as PlayerWeaponController;

            _adj = new IsoCamAdjust();

            var skills = _playerWeaponCtrl.GetSkillIDs(); // run later 

            skills.Add(_shieldSkillID);
            _skillCtrl = new SkillsController(skills);
            _shieldCtrl = new ShieldController("player"); // todo placeholder


            _handler.RegisterUnitForStatUpdates(_dodgeCtrl);
            _handler.RegisterUnitForStatUpdates(_skillCtrl);
            _handler.RegisterUnitForStatUpdates(_shieldCtrl);

            _controls.Game.Dash.performed += Dash_performed;
            _controls.Game.SkillE.performed += SkillE_performed;
            _controls.Game.SkillQ.performed += SkillQ_performed;
            _controls.Game.SkillR.performed += SkillR_performed;
            _controls.Game.MainAttack.performed += MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed += RangedAttack_performed;

            _playerWeaponCtrl.SwitchAnimationLayersEvent += DoSwitchLayer;

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

            _playerWeaponCtrl.SwitchAnimationLayersEvent -= DoSwitchLayer;
        }

    }

    #region controller checks

    private void RangedAttack_performed(CallbackContext obj)
    {
        if (_playerWeaponCtrl.UseWeaponCheck(WeaponType.Ranged))
        { 
            CombatActionSuccessEvent?.Invoke(CombatActionType.Ranged); 
            
        }

    }
    private void MeleeAttack_performed(CallbackContext obj)
    {
        if (_playerWeaponCtrl.UseWeaponCheck(WeaponType.Melee))
            CombatActionSuccessEvent?.Invoke(CombatActionType.Melee);
    }
    private void SkillR_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.ShieldSpecialR))
        {
            CombatActionSuccessEvent?.Invoke(CombatActionType.ShieldSpecialR);
        }
    }
    private void SkillQ_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.MeleeSpecialQ))
        {
            CombatActionSuccessEvent?.Invoke(CombatActionType.MeleeSpecialQ);
        }
    }
    private void SkillE_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.RangedSpecialE))
        {
            CombatActionSuccessEvent?.Invoke(CombatActionType.RangedSpecialE);
        }
    }
    private void Dash_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_dodgeCtrl.IsDodgePossibleCheck())
        {
            CombatActionSuccessEvent?.Invoke(CombatActionType.Dodge);
        }
    }
    #endregion

    private void Update()
    {
        CalculateMovement();

        if (_selector.CalculateAimPoint() == null) return;

        lookTarget =(Vector3)_selector.CalculateAimPoint();

        _aim.SetLookTarget(lookTarget);
        transform.LookAt(lookTarget);
    }

    private void CalculateMovement()
    {
        var input = _controls.Game.WASD.ReadValue<Vector2>();
        Vector3 AD = _adj.Isoright * input.x;
        Vector3 WS = _adj.Isoforward * input.y;
        velocityVector = AD + WS;  
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + MoveDirection);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward + transform.position);

    }


}