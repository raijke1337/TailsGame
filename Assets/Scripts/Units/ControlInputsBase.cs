using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : MonoBehaviour
{
    [Inject] protected StatsUpdatesHandler _handler;
    
    protected BaseWeaponController _weaponCtrl;
    public BaseWeaponController GetWeaponController => _weaponCtrl;
    public virtual event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;

    [HideInInspector] public NavMeshAgent NavMeshAg;


    protected virtual void OnEnable()
    {
        _weaponCtrl = GetComponent<BaseWeaponController>();
        NavMeshAg = GetComponent<NavMeshAgent>();

        BindControllers(true);
    }

    protected virtual void BindControllers(bool isEnable)
    {
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
    }


    public ref Vector3 MoveDirection => ref velocityVector;
    protected Vector3 velocityVector;

}

