using Assets.Scripts.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : MonoBehaviour, ITakesTriggers
{
    protected BaseUnit Unit;
    public void SetUnit(BaseUnit u) => Unit = u;
    public abstract UnitType GetUnitType();

    public bool IsControlsBusy { get; set; } // todo ?


    [Inject] protected StatsUpdatesHandler _handler;

    [SerializeField] protected ItemEmpties Empties;
    public ItemEmpties GetEmpties => Empties;

    protected UnitInventoryComponent _inventoryManager;
    protected BaseStatsController _statsCtrl;
    protected WeaponController _weaponCtrl;
    protected DodgeController _dodgeCtrl;
    protected ShieldController _shieldCtrl;
    protected SkillsController _skillCtrl;
    protected ComboController _comboCtrl;
    protected StunsController _stunsCtrl;

    public DodgeController GetDodgeController => _dodgeCtrl;
    public ShieldController GetShieldController => _shieldCtrl;
    public BaseStatsController GetStatsController => _statsCtrl;
    public SkillsController GetSkillsController => _skillCtrl;
    public WeaponController GetWeaponController => _weaponCtrl;

    [SerializeField] protected string[] extraSkills;


    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    public event SimpleEventsHandler StaggerHappened;

    protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
    protected void StunEventCallback() => StaggerHappened?.Invoke();


    public virtual void InitControllers(BaseStatsController stats)
    {
        _statsCtrl = stats;

        _inventoryManager = GetComponent<UnitInventoryComponent>();

        _shieldCtrl = new ShieldController(Empties);
        _dodgeCtrl = new DodgeController(Empties); 
        _weaponCtrl = new WeaponController(Empties);
        _skillCtrl = new SkillsController(Empties);
        _comboCtrl = new ComboController(Unit.GetID);
        _stunsCtrl = new StunsController(); // using default "default" for now 
    }

    public virtual void BindControllers(bool isEnable)
    {

        
        _inventoryManager.AddItemUser(_weaponCtrl);
        _inventoryManager.AddItemUser(_shieldCtrl);
        _inventoryManager.AddItemUser(_dodgeCtrl);
        // todo proper inventory

        IsControlsBusy = false;
        _weaponCtrl.Owner  = Unit;

        _handler.RegisterUnitForStatUpdates(_inventoryManager, isEnable);
        // inv manager loads items into managers and they set their isReady status

        _handler.RegisterUnitForStatUpdates(_dodgeCtrl,isEnable);  // maybe todo: turn dodge into a SelfSkill
        _handler.RegisterUnitForStatUpdates(_shieldCtrl,isEnable);
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
        _handler.RegisterUnitForStatUpdates(_stunsCtrl, isEnable);   
        _handler.RegisterUnitForStatUpdates(_statsCtrl, isEnable);
        _handler.RegisterUnitForStatUpdates(_comboCtrl, isEnable);
        // only registered if (isReady)

        // needs data from set up weaponctrl
        List<string> skills = (List<string>)_weaponCtrl.GetSkillStrings();
        foreach (var skill in extraSkills) { skills.Add(skill); }
        skills.AddRange(_shieldCtrl.GetSkillStrings());

        _skillCtrl.LoadSkills(skills);
        _handler.RegisterUnitForStatUpdates(_skillCtrl, isEnable);
        _stunsCtrl.StunHappenedEvent += StunEventCallback;

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


    private void OnDisable()
    {
        BindControllers(false);
    }

    public void AddTriggeredEffect(TriggeredEffect eff)
    {
        switch (eff.StatType)
        {
            case TriggerChangedValue.Health:
                if (!_shieldCtrl.IsReady)
                {
                    _statsCtrl.AddTriggeredEffect(eff);
                }
                else
                {
                    _statsCtrl.AddTriggeredEffect(_shieldCtrl.ProcessHealthChange(eff));
                }
                break;
            case TriggerChangedValue.Shield:
                _shieldCtrl.AddTriggeredEffect(eff);
                break;
            case TriggerChangedValue.Combo:
                _comboCtrl.AddTriggeredEffect(eff);
                break;
            case TriggerChangedValue.MoveSpeed:
                _statsCtrl.AddTriggeredEffect(eff);
                break;
            case TriggerChangedValue.TurnSpeed:
                _statsCtrl.AddTriggeredEffect(eff);
                break;
            case TriggerChangedValue.Stagger:
                _stunsCtrl.AddTriggeredEffect(eff);
                break;
        }
    }
}

