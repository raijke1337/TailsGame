using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : MonoBehaviour
{
    [Inject] protected StatsUpdatesHandler _handler;
    public bool IsControlsBusy { get; set; } // todo not very good solution

    [SerializeField] protected WeaponController _weaponCtrl;
    [SerializeField] protected StunnerComponent _staggerCheck;


    protected BaseStatsController _statsCtrl;
    public WeaponController GetWeaponController => _weaponCtrl;
    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
    public event SimpleEventsHandler StaggerHappened;

    protected virtual void OnEnable()
    {
        IsControlsBusy = false;
        _staggerCheck = new StunnerComponent(3, 3); // todo configs
        _weaponCtrl = GetComponent<WeaponController>(); // todo remove this (mono)
        BindControllers(true);
    }

    protected virtual void BindControllers(bool isEnable)
    {
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
        _handler.RegisterUnitForStatUpdates(_staggerCheck, isEnable);
    }

    public void SetStatsController(BaseStatsController stats)
    {
        _statsCtrl = stats;
        _statsCtrl.GetBaseStats[StatType.Health].ValueChangedEvent += StaggerCheck;
    }

    public ref Vector3 MoveDirection => ref velocityVector;
    protected Vector3 velocityVector;

    protected void StaggerCheck(float current,float prev)
    {
        if (prev - current >= Constants.Combat.c_StaggeringHitHealthPercent * _statsCtrl.GetBaseStats[StatType.Health].GetMax()) ;
        {
            if (_staggerCheck.DidHitStun())
            {
                StaggerHappened?.Invoke();
            }
        }
    }

}

