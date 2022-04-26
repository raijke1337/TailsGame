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

    [SerializeField] private DodgeController _dodgeCtrl;
    [SerializeField] private ShieldController _shieldCtrl;

    private SelectorComponent _selector;


    public DodgeController GetDodgeController => _dodgeCtrl;
    public ShieldController GetShieldController => _shieldCtrl;

    public event SimpleEventsHandler<WeaponType> ChangeLayerEvent;


    private void OnDisable()
    {
        PlayerBind(false);
    }
    private IsoCamAdjust _adj;

    [SerializeField] private CrosshairScript _aim;

    private Vector3 lookTarget;

    public override void BindControllers(bool isEnable)
    {
        base.BindControllers(isEnable);
        _controls = new PlayerControls();
        _shieldCtrl = new ShieldController(_statsCtrl.GetUnitID);
        _dodgeCtrl = new DodgeController(_statsCtrl.GetUnitID);
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

            //_playerWeaponCtrl = _weaponCtrl as PlayerWeaponController;

            _adj = new IsoCamAdjust();



            _handler.RegisterUnitForStatUpdates(_dodgeCtrl);
            _handler.RegisterUnitForStatUpdates(_shieldCtrl);

            _controls.Game.Dash.performed += Dash_performed;
            _controls.Game.SkillE.performed += SkillE_performed;
            _controls.Game.SkillQ.performed += SkillQ_performed;
            _controls.Game.SkillR.performed += SkillR_performed;
            _controls.Game.MainAttack.performed += MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed += RangedAttack_performed;

            _weaponCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;

            _aim = GetComponentInChildren<CrosshairScript>();
            _aim.transform.SetParent(null);
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

            _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
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

    private void RangedAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Ranged))
            CombatActionSuccessCallback(CombatActionType.Ranged);             
        
    }
    private void MeleeAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee))
            CombatActionSuccessCallback(CombatActionType.Melee);
    }
    private void SkillR_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.ShieldSpecialR,out var c))
        {
            if (_statsCtrl.RequestHeatUsage(c))
            CombatActionSuccessCallback(CombatActionType.ShieldSpecialR);
        }
    }
    private void SkillQ_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.MeleeSpecialQ, out var c))
        {
            if (_statsCtrl.RequestHeatUsage(c))
                CombatActionSuccessCallback(CombatActionType.MeleeSpecialQ);
        }
    }
    private void SkillE_performed(CallbackContext obj)
    {
        if (IsControlsBusy) return;
        if (_skillCtrl.RequestSkill(CombatActionType.RangedSpecialE, out var c))
        {
            if (_statsCtrl.RequestHeatUsage(c))
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
        if (_controls == null || IsControlsBusy) return;
        var input = _controls.Game.WASD.ReadValue<Vector2>();
        Vector3 AD = _adj.Isoright * input.x;
        Vector3 WS = _adj.Isoforward * input.y;
        velocityVector = AD + WS;  
    }
    private void Aiming()
    {
        if (_selector == null || _selector.CalculateAimPoint()==null) return;
        lookTarget = (Vector3)_selector.CalculateAimPoint();
        _aim.SetLookTarget(lookTarget);
        transform.LookAt(lookTarget);

        var currentX = transform.localEulerAngles.x;
        transform.Rotate(new Vector3(-currentX,0,0));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + MoveDirection);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward + transform.position);

    }


}