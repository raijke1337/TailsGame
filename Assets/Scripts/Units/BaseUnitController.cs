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

[RequireComponent(typeof(BaseUnit))]
public abstract class BaseUnitController : MonoBehaviour
{
    protected BaseWeaponController _weaponCtrl;
    public BaseUnit GetUnit { get; protected set; }
    public virtual event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    public BaseWeaponController GetWeaponController => _weaponCtrl;

    protected virtual void Awake()
    {
        _weaponCtrl = GetComponent<BaseWeaponController>();

        GetUnit = GetComponent<BaseUnit>();
        GetUnit.UnitDiedEvent += UnitDiedAction;
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

