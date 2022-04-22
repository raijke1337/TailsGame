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
    [SerializeField] protected string[] extraSkills;

    [SerializeField] protected SkillsController _skillCtrl;
    public SkillsController GetSkillsController => _skillCtrl;
    protected BaseStatsController _statsCtrl;
    public WeaponController GetWeaponController => _weaponCtrl;
    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
    public event SimpleEventsHandler StaggerHappened;

    public virtual void BindControllers(bool isEnable)
    {
        IsControlsBusy = false;
        _staggerCheck = new StunnerComponent(3, 3); // todo configs
        _weaponCtrl = GetComponent<WeaponController>(); // todo remove this (mono)
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
        _handler.RegisterUnitForStatUpdates(_staggerCheck, isEnable);
        _handler.RegisterUnitForStatUpdates(_skillCtrl, isEnable);
        var skills = _weaponCtrl.GetSkillIDs();
        foreach (var skill in extraSkills) { skills.Add(skill); }
        _skillCtrl = new SkillsController(skills);
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
        if (prev - current >= Constants.Combat.c_StaggeringHitHealthPercent * _statsCtrl.GetBaseStats[StatType.Health].GetMax())
        {
            if (_staggerCheck.DidHitStun())
            {
                StaggerHappened?.Invoke();
            }
        }
    }
    private void OnDisable()
    {
        BindControllers(false);
    }

}

