using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : MonoBehaviour
{
    [Inject] protected StatsUpdatesHandler _handler;
    public bool IsControlsBusy { get; set; } // todo not very good solution


    protected BaseWeaponController _weaponCtrl;
    public BaseWeaponController GetWeaponController => _weaponCtrl;
    public virtual event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;


    protected virtual void OnEnable()
    {
        IsControlsBusy = false;
        _weaponCtrl = GetComponent<BaseWeaponController>(); // todo remove this (mono)
        BindControllers(true);
    }

    protected virtual void BindControllers(bool isEnable)
    {
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
    }


    public ref Vector3 MoveDirection => ref velocityVector;
    protected Vector3 velocityVector;

}

