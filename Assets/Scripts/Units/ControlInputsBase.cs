using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : MonoBehaviour
{
    private BaseUnit unit;
    public void SetUnit(BaseUnit u) => unit = u;

    [Inject] protected StatsUpdatesHandler _handler;
    public bool IsControlsBusy { get; set; } // todo ?

    protected WeaponController _weaponCtrl;
    [SerializeField] protected StunnerComponent _staggerCheck;

    [SerializeField] protected string[] extraSkills;

    [SerializeField] protected SkillsController _skillCtrl;
    public SkillsController GetSkillsController => _skillCtrl;
    protected BaseStatsController _statsCtrl;
    public WeaponController GetWeaponController => _weaponCtrl;
    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
    public event SimpleEventsHandler StaggerHappened;


    public virtual void InitControllers(BaseStatsController stats)
    {
        _statsCtrl = stats;
        _statsCtrl.GetBaseStats[BaseStatType.Health].ValueChangedEvent += StaggerCheck;
        _staggerCheck = new StunnerComponent(3, 3); // todo configs
        _weaponCtrl = GetComponent<WeaponController>(); // todo remove this (mono)
        // not initialized? todo

    }
    public virtual void BindControllers(bool isEnable)
    {
        IsControlsBusy = false;
        _weaponCtrl.SetUser(unit);
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
        _handler.RegisterUnitForStatUpdates(_staggerCheck, isEnable);


        // needs data from set up weaponctrl
        var skills = _weaponCtrl.GetSkillIDs();
        foreach (var skill in extraSkills) { skills.Add(skill); }
        _skillCtrl = new SkillsController(skills);

        _handler.RegisterUnitForStatUpdates(_skillCtrl, isEnable);
    }

    public void ToggleBusyControls_AnimationEvent(int state)
    {
        IsControlsBusy = state != 0;
    }   

    public ref Vector3 MoveDirection => ref velocityVector;
    protected Vector3 velocityVector;

    protected void LerpRotateToTarget(Vector3 looktarget)
    {
        Vector3 relativePosition = looktarget - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
            Time.deltaTime * _statsCtrl.GetBaseStats[BaseStatType.TurnSpeed].GetCurrent);
    }


    protected void StaggerCheck(float current,float prev)
    {
        if (prev - current >= Constants.Combat.c_StaggeringHitHealthPercent * _statsCtrl.GetBaseStats[BaseStatType.Health].GetMax)
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

