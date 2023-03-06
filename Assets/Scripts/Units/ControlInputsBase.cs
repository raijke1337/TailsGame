using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseUnit))]
public abstract class ControlInputsBase : ManagedControllerBase, ITakesTriggers
{
    protected BaseUnit Unit;

    public abstract UnitType GetUnitType();

    [SerializeField] public bool IsControlsBusy; // todo ?


    protected List<IStatsComponentForHandler> _controllers = new List<IStatsComponentForHandler>();

    public void SetUnit(BaseUnit u) => Unit = u;

    protected float lastDelta;

    [SerializeField] protected ItemEmpties Empties;
    public ItemEmpties GetEmpties => Empties;

    [SerializeField] protected BaseStatsController _statsCtrl;
    [SerializeField] protected WeaponController _weaponCtrl;
    [SerializeField] protected DodgeController _dodgeCtrl;
    [SerializeField] protected ShieldController _shieldCtrl;
    [SerializeField] protected SkillsController _skillCtrl;
    [SerializeField] protected ComboController _comboCtrl;
    [SerializeField] protected StunsController _stunsCtrl;

    public DodgeController GetDodgeController => _dodgeCtrl;
    public ShieldController GetShieldController => _shieldCtrl;
    public BaseStatsController GetStatsController => _statsCtrl;
    public SkillsController GetSkillsController => _skillCtrl;
    public WeaponController GetWeaponController => _weaponCtrl;


    public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
    public event SimpleEventsHandler StaggerHappened;
    protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
    protected void StunEventCallback() => StaggerHappened?.Invoke();
    #region scenes

    public Vector3 InputDirectionOverride = Vector3.zero;
    #endregion

    #region ManagedController
    public override void UpdateController(float delta)
    {
        lastDelta = delta;
    }
    public override void StartController()
    {
        _statsCtrl = new BaseStatsController(Unit.GetID);
        MoveDirectionFromInputs = InputDirectionOverride;
        if (InputDirectionOverride != Vector3.zero) return; // Scene

        _comboCtrl = new ComboController(Unit.GetID);
        _stunsCtrl = new StunsController();
        _shieldCtrl = new ShieldController(Empties);
        _dodgeCtrl = new DodgeController(Empties);
        _weaponCtrl = new WeaponController(Empties);
        _skillCtrl = new SkillsController(Empties);


        _controllers.Add(_shieldCtrl);
        _controllers.Add(_dodgeCtrl);
        _controllers.Add(_weaponCtrl);
        _controllers.Add(_skillCtrl);
        _controllers.Add(_comboCtrl);
        _controllers.Add(_stunsCtrl);
        _controllers.Add(_statsCtrl);

        foreach (var ctrl in _controllers)
        {
            ctrl.ComponentChangedStateToEvent += RegisterController;
            if (ctrl.IsReady) ctrl.Ping();
        }

        IsControlsBusy = false;
        _weaponCtrl.Owner = Unit;
        _stunsCtrl.StunHappenedEvent += StunEventCallback;
    }
    public override void StopController()
    {
        foreach (var ctrl in _controllers)
        {
            ctrl.ComponentChangedStateToEvent -= RegisterController;
        }
        _stunsCtrl.StunHappenedEvent -= StunEventCallback;
    }
    #endregion


    protected void RegisterController(bool isEnable, IStatsComponentForHandler cont)
    {
        GameManager.Instance.GetGameControllers.StatsUpdatesHandler.RegisterUnitForStatUpdates(cont, isEnable);
    }
    public void AddSkillString(string name, bool isAdd = true) => _skillCtrl.UpdateSkills(name, isAdd);

    public void ToggleBusyControls_AnimationEvent(int state)
    {
        IsControlsBusy = state != 0;
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

    #region movement



    public ref Vector3 GetMoveDirection => ref MoveDirectionFromInputs;
    protected Vector3 MoveDirectionFromInputs;

    protected virtual void LerpRotateToTarget(Vector3 looktarget, float delta)
    {
        Vector3 relativePosition = looktarget - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
            delta * _statsCtrl.GetBaseStats[BaseStatType.TurnSpeed].GetCurrent);
    }

    public void PerformDodging()
    {
        _dodgeCor = StartCoroutine(DodgingMovement());
    }

    private Coroutine _dodgeCor;
    // stop the dodge like this

    private void OnCollisionEnter(Collision collision)
    {
        if (_dodgeCor != null && !collision.gameObject.CompareTag("Ground"))
        {
            IsControlsBusy = false;
            StopCoroutine(_dodgeCor);
        }
    }

    private IEnumerator DodgingMovement()
    {
        var stats = _dodgeCtrl.GetDodgeStats;
        IsControlsBusy = true;

        Vector3 start = transform.position;
        Vector3 end = start + GetMoveDirection * stats[DodgeStatType.Range].GetCurrent;

        float p = 0f;
        while (p <= 1f)
        {
            p += Time.deltaTime * stats[DodgeStatType.Speed].GetCurrent;
            transform.position = Vector3.Lerp(start, end, p);
            yield return null;
        }
        IsControlsBusy = false;
        yield return null;
    }


    #endregion

}

