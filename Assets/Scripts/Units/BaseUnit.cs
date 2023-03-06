using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ControlInputsBase))]

public abstract class BaseUnit : MonoBehaviour, IHasID, ITakesTriggers
{
    [SerializeField] protected string StatsID;

    public string GetID => StatsID;

    protected Animator _animator;
    protected Rigidbody _rigidbody;
    public Collider GetCollider { get; private set; }
    protected ControlInputsBase _controller;

    public Side Side;

    public UnitType GetUnitType() => _controller.GetUnitType();
    public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetStats() => _controller.GetStatsController.GetBaseStats;

    public T GetInputs<T>() where T : ControlInputsBase => _controller as T;

    public string GetFullName => _controller.GetStatsController.GetDisplayName;

    public event SimpleEventsHandler<BaseUnit> BaseUnitDiedEvent;
    public event SkillRequestedEvent SkillRequestSuccessEvent;
    protected void SkillRequestCallBack(string id, BaseUnit unit) => SkillRequestSuccessEvent?.Invoke(id, unit, unit._controller.GetEmpties.SkillsEmpty);

    protected Camera _faceCam;
    protected void ToggleCamera(bool value) { _faceCam.enabled = value; }
    public Camera GetFaceCam { get => _faceCam; }

    //protected bool bindsComplete = false;

    public virtual void InitiateUnit() // this is run by unit manager
    {
        UpdateComponents();

        _faceCam.enabled = false;

        switch (GameManager.Instance.GetCurrentLevelData.Type)
        {
            case LevelType.Scene:
                _animator.SetLayerWeight(_animator.GetLayerIndex("Scene"), 100f);
                _controller.SetUnit(this);
                _controller.StartController();
                break;
            case LevelType.Menu:
                break;
            case LevelType.Game:
                _controller.SetUnit(this);
                _controller.StartController();
                _animator.SetLayerWeight(3, 0);
                ControllerEventsBinds(true);
                break;
        }
        //Debug.Log($"Initiated {GetFullName}");
    }

    public virtual void DisableUnit()
    {
        ControllerEventsBinds(false);
        _controller.StopController();
    }

    protected virtual void HandleStartingEquipment(IEquippable item) // equipment can't be changed mid-level so it's no problem here that this is run once
    {
        UpdateComponents();

        string skill = item.GetContents.SkillString;
        if (skill.Length != 0) _controller.AddSkillString(skill);

        switch (item.GetContents.ItemType)
        {
            case EquipItemType.None:
                Debug.Log($"No logic to equip {item} {item.GetContents.ItemType}");
                break;
            case EquipItemType.MeleeWeap:
                _controller.GetWeaponController.LoadItem(item);
                break;
            case EquipItemType.RangedWeap:
                _controller.GetWeaponController.LoadItem(item);
                break;
            case EquipItemType.Shield:
                _controller.GetShieldController.LoadItem(item);
                break;
            case EquipItemType.Booster:
                _controller.GetDodgeController.LoadItem(item);
                break;
            case EquipItemType.Other:
                Debug.Log($"No logic to equip {item} {item.GetContents.ItemType}");
                break;
        }
    }

    protected virtual void UpdateComponents()
    {
        if (GetCollider == null) GetCollider = GetComponent<Collider>();
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_controller == null) _controller = GetComponent<ControlInputsBase>();
        if (_faceCam == null) _faceCam = GetComponentsInChildren<Camera>().First(t => t.CompareTag("FaceCamera"));
    }


    protected virtual void ControllerEventsBinds(bool isEnable)
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



    public virtual void RunUpdate(float delta)
    {
        if (_controller == null)
        {
            Debug.LogWarning($"Unit {this} was not initialized!");
            return;
        }
        AnimateMovement();
        if (GameManager.Instance.GetCurrentLevelData.Type != LevelType.Game) return;
        _controller.UpdateController(delta);
    }
    #region stats
    protected virtual void OnDeath()
    {
        _animator.SetTrigger("Death");
        BaseUnitDiedEvent?.Invoke(this);
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
        Debug.Log(movement);
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
        if (GameManager.Instance.GetCurrentLevelData.Type != LevelType.Game) return;
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
        if (_controller == null) return; //in case we have a scene

        bool result = value > 0;
        _controller.GetWeaponController.ToggleTriggersOnMelee(result);
    }

    protected virtual void AnimateStagger()
    {
        _animator.SetTrigger("TakeDamage");
        Debug.Log($"{_controller.GetStatsController.GetDisplayName} got stunned!");
    }


    #endregion










}
