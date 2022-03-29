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

[RequireComponent(typeof(BaseUnit))]
public abstract class BaseUnitController : MonoBehaviour
{
    [Inject] protected StatsUpdatesHandler _handler;
    protected BaseWeaponController _weaponCtrl;
    public BaseUnit Unit { get; protected set; }
    public virtual event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    public BaseWeaponController GetWeaponController => _weaponCtrl;

    protected virtual void Awake()
    {
        Unit = GetComponent<BaseUnit>();
        Unit.UnitDiedEvent += UnitDiedAction;
        _weaponCtrl = GetComponent<BaseWeaponController>();
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, true);
    }
    private void OnDisable()
    {
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, false);
    }


    public ref Vector3 MoveDirection => ref _movement;
    protected Vector3 _movement;

    protected abstract void UnitDiedAction(BaseUnit unit);


    #region controller checks

    protected virtual void RangedAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Ranged))
            CombatActionSuccessEvent?.Invoke(CombatActionType.Ranged);
    }
    protected virtual void MeleeAttack_performed(CallbackContext obj)
    {
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee))
            CombatActionSuccessEvent?.Invoke(CombatActionType.Melee);
    }
    #endregion

}

