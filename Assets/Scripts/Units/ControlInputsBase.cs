using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : MonoBehaviour
{
    [Inject] protected StatsUpdatesHandler _handler;
    public bool IsControlsBusy { get; set; } // todo not very good solution

    [SerializeField] protected WeaponController _weaponCtrl;

    protected BaseStatsController _statsCtrl;
    public WeaponController GetWeaponController => _weaponCtrl;
    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);

    protected virtual void OnEnable()
    {
        IsControlsBusy = false;
        _weaponCtrl = GetComponent<WeaponController>(); // todo remove this (mono)
        BindControllers(true);
    }

    protected virtual void BindControllers(bool isEnable)
    {
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
    }

    public void SetStatsController(BaseStatsController stats) => _statsCtrl = stats;

    public ref Vector3 MoveDirection => ref velocityVector;
    protected Vector3 velocityVector;

}

