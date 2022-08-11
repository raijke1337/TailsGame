
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ControlInputsBase),typeof(UnitInventoryComponent))]

public abstract class BaseUnit : MonoBehaviour, IHasID, ITakesTriggers
{
    [SerializeField] protected string StatsID;

    [Inject] protected StatsUpdatesHandler _handler;
    public string GetID => StatsID;

    protected Animator _animator;
    protected Rigidbody _rigidbody;
    public Collider GetCollider { get; private set; }
    protected ControlInputsBase _controller;

    public Side Side;

    public UnitType GetUnitType() => _controller.GetUnitType();
    public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetStats() => _controller.GetStatsController.GetBaseStats;

    public T GetInputs<T>() where T : ControlInputsBase => _controller as T;
    public UnitInventoryComponent Inventory { get; protected set; }


    public string GetFullName => _controller.GetStatsController.GetDisplayName;



    public event SimpleEventsHandler<BaseUnit> BaseUnitDiedEvent;
    public event SkillRequestedEvent SkillRequestSuccessEvent;
    protected void SkillRequestCallBack(string id, BaseUnit unit) => SkillRequestSuccessEvent?.Invoke(id, unit,unit._controller.GetEmpties.SkillsEmpty);

    private Camera _faceCam;
    public void ToggleCamera(bool value) { _faceCam.enabled = value; }

    #region setups

    protected virtual void Awake()
    {
        Inventory = GetComponent<UnitInventoryComponent>(); //todo placeholder
    }
    protected virtual void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        _controller = GetComponent<ControlInputsBase>();
        _controller.SetUnit(this);
        _controller.InitControllers(new BaseStatsController(StatsID));

        _controller.BindControllers(true);

        _faceCam = GetComponentsInChildren<Camera>().First(t => t.CompareTag("FaceCamera"));
        _faceCam.enabled = false;

        GetCollider = GetComponent<Collider>();
    }
    protected virtual void UnitBinds(bool isEnable)
    {
        if (isEnable)
        {
            _controller.GetStatsController.UnitDiedEvent += OnDeath;
            _controller.CombatActionSuccessEvent += (t) => AnimateCombatActivity(t);
            _controller.StaggerHappened += AnimateStagger; 
        }
        else
        {
            _controller.GetStatsController.UnitDiedEvent -= OnDeath;
            _controller.CombatActionSuccessEvent -= (t) => AnimateCombatActivity(t);
            _controller.StaggerHappened -= AnimateStagger;
        }
    }
    protected virtual void Start()
    {
        UnitBinds(true);
    }

    protected virtual void OnDisable()
    {
        UnitBinds(false);
        _controller.BindControllers(false);
    }
    protected virtual void FixedUpdate()
    {   
        AnimateMovement();
    }
    #endregion
    #region stats
    protected virtual void OnDeath()
    {

            _animator.SetTrigger("Death");
            BaseUnitDiedEvent?.Invoke(this);
            UnitBinds(false);
            _controller.IsControlsBusy = true;
            StartCoroutine(ItemDisappearsCoroutine(Constants.Combat.c_RemainsDisappearTimer, gameObject));        
    }
    public virtual void AddTriggeredEffect(TriggeredEffect eff)
    {
        _controller.AddTriggeredEffect(eff);
    }
    #endregion
    #region movement
    //sets animator values 
    protected virtual void AnimateMovement()
    {
        ref var movement = ref _controller.MoveDirection;
        if (movement.x == 0f && movement.z == 0f)
        {
            _animator.SetBool("Moving", false);
            _animator.SetFloat("ForwardMove", 0f);
            _animator.SetFloat("SideMove", 0f);
        }
        else
        {
            _animator.SetBool("Moving", true);
            CalcAnimVector(movement);
        }
    }
    //  calculates the vector which is used to set values in animator
    protected void CalcAnimVector(Vector3 vector)
    {
        var playerFwd = transform.forward;
        var playerRght = transform.right;
        vector.y = 0;
        vector.Normalize();
        // Dot product of two vectors determines how much they are pointing in the same direction.
        // If the vectors are normalized (transform.forward and right are)
        // then the value will be between -1 and +1.
        var x = Vector3.Dot(playerRght, vector);
        var z = Vector3.Dot(playerFwd, vector);

        _animator.SetFloat("ForwardMove", z);
        _animator.SetFloat("SideMove", x);
    }
    #endregion
    #region combat
    protected virtual void AnimateCombatActivity(CombatActionType type)
    {
        switch (type)
        {
            case CombatActionType.Melee:
                _animator.SetTrigger("MeleeAttack");
                break;
            case CombatActionType.Ranged:
                _animator.SetTrigger("RangedAttack");
                break;
            case CombatActionType.Dodge:
                _animator.SetTrigger("Dodge");
                _controller.PerformDodging();
                break;
            case CombatActionType.MeleeSpecialQ:
                _animator.SetTrigger("QSpecial");
                SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(type), this);
                break;
            case CombatActionType.RangedSpecialE:
                _animator.SetTrigger("ESpecial");
                SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(type), this);
                break;
            case CombatActionType.ShieldSpecialR:
                _animator.SetTrigger("RSpecial");
                SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(type), this);
                break;
        }
    }

    public virtual void TriggerTogglingEvent_UE(float value)
    {    // 1 on start 0 on end
        bool result = value > 0;
        _controller.GetWeaponController.ToggleTriggersOnMelee(result);
    }

    protected virtual void AnimateStagger()
    {  
        _animator.SetTrigger("TakeDamage");
        Debug.Log($"{_controller.GetStatsController.GetDisplayName} got stunned!");
    }


    #endregion

    #region misc
    public static IEnumerator ItemDisappearsCoroutine(float time, GameObject item)
    {
        float passed = 0f;
        while (passed < time)
        {
            passed += Time.deltaTime;
            yield return null;
        }
        Destroy(item);
        yield return null;
    }
    #endregion

}
