using Assets.Scripts.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : MonoBehaviour
{
    protected BaseUnit Unit;
    public void SetUnit(BaseUnit u) => Unit = u;


    public bool IsControlsBusy { get; set; } // todo ?


    [Inject] protected StatsUpdatesHandler _handler;
    [SerializeField] protected ItemEmpties Empties;
    public ItemEmpties GetEmpties => Empties;
    // for skills todo

    protected InventoryManager _inventoryManager;
    protected BaseStatsController _statsCtrl;
    protected WeaponController _weaponCtrl;
    protected DodgeController _dodgeCtrl;
    protected ShieldController _shieldCtrl;
    protected SkillsController _skillCtrl;

    public DodgeController GetDodgeController => _dodgeCtrl;
    public ShieldController GetShieldController => _shieldCtrl;


    [SerializeField] protected StunnerComponent _staggerCheck;
    public event SimpleEventsHandler StaggerHappened;

    [SerializeField] protected string[] extraSkills;

    public SkillsController GetSkillsController => _skillCtrl;
    public WeaponController GetWeaponController => _weaponCtrl;

    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
    public abstract UnitType GetUnitType();



    public virtual void InitControllers(BaseStatsController stats)
    {
        _statsCtrl = stats;
        _staggerCheck = new StunnerComponent(3, 3); // todo configs

        _inventoryManager = GetComponent<InventoryManager>();


        _shieldCtrl = new ShieldController(Empties);
        _dodgeCtrl = new DodgeController(Empties); 
        _weaponCtrl = new WeaponController(Empties);
        _skillCtrl = new SkillsController(Empties);

    }

    public virtual void BindControllers(bool isEnable)
    {

        
        _inventoryManager.AddItemUser(_weaponCtrl);
        _inventoryManager.AddItemUser(_shieldCtrl);
        _inventoryManager.AddItemUser(_dodgeCtrl);
        // todo proper inventory


        _statsCtrl.GetBaseStats[BaseStatType.Health].ValueChangedEvent += StaggerCheck;

        IsControlsBusy = false;
        _weaponCtrl.Owner  = Unit;

        _handler.RegisterUnitForStatUpdates(_inventoryManager, isEnable);
        // inv manager loads items into managers and they set their isReady status

        _handler.RegisterUnitForStatUpdates(_dodgeCtrl,isEnable);
        _handler.RegisterUnitForStatUpdates(_shieldCtrl,isEnable);
        _handler.RegisterUnitForStatUpdates(_weaponCtrl, isEnable);
        _handler.RegisterUnitForStatUpdates(_staggerCheck, isEnable);   
        // only registered if (isReady)


        // needs data from set up weaponctrl
        List<string> skills = (List<string>)_weaponCtrl.GetSkillStrings();
        foreach (var skill in extraSkills) { skills.Add(skill); }

        _skillCtrl.LoadSkills(skills);
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

