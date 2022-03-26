using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Zenject;

public abstract class BaseUnit : MonoBehaviour
{
    protected BaseStatsController _baseStats;
    protected BaseUnitController _controller;
    [Inject] protected StatsUpdatesHandler _handler;

    public string BaseStatsID;

    protected Animator _animator;
    protected Rigidbody _rigidbody;
    
    public string GetFullName() => _baseStats.GetDisplayName;
    public IReadOnlyDictionary<StatType,StatValueContainer> GetStats() => _baseStats.GetBaseStats;     
    
    public event SimpleEventsHandler<BaseUnit> UnitDiedEvent;

    private Camera _faceCam;
    public void ToggleCamera(bool value){ _faceCam.enabled = value; }

    public void ApplyEffect(TriggeredEffect eff)
    {
        _baseStats.AddTriggeredEffect(eff);
    }

    protected virtual void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<BaseUnitController>();
        _rigidbody = GetComponent<Rigidbody>();

        _baseStats = new BaseStatsController(BaseStatsID);
        _handler.RegisterUnitForStatUpdates(_baseStats);


        _handler.RegisterUnitForStatUpdates(_controller.GetWeaponController);

        UnitBinds(true);

        _faceCam = GetComponentsInChildren<Camera>().First(t=>t.CompareTag("FaceCamera"));
        _faceCam.enabled = false;
    }

    protected virtual void OnDisable()
    {
        UnitBinds(false);
    }
    protected virtual void Update()
    {
        AnimateMovement();
    }
    public void OnAnimationComplete()
    {

    }
    //take damage and die here
    protected void HealthChangedEvent(float value)
    {
        if (value <= 0f)
        {
            _animator.SetTrigger("Death");
            Debug.Log($"{name} died");
            UnitDiedEvent?.Invoke(this);
        }
        else
        {
            _animator.SetTrigger("TakeDamage");
        }
    }

    //sets animator values and moves transfrom
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
            // transform.position += GetStats()[StatType.MoveSpeed].GetCurrent() * Time.deltaTime * movement; // Removed because we will be using navmeshagent for npcs and rigidbody for player
            CalcAnimVector(movement);
        }
    }
    protected virtual void AnimateCombatActivity(CombatActionType type)
    {
        Debug.Log($"Animating: {type} by {GetFullName()}");
        switch (type)
        {
            case CombatActionType.Melee:
                _animator.SetTrigger("MeleeAttack");
                break;
            case CombatActionType.Ranged:
                _animator.SetTrigger("RangedAttack");
                break;
            case CombatActionType.Dodge:
                break;
            case CombatActionType.MeleeSpecialQ:
                break;
            case CombatActionType.RangedSpecialE:
                break;
            case CombatActionType.ShieldSpecialR:
                break;
        }
    }
        //  calculates the vector which is used to set values in animator
        protected Vector3 CalcAnimVector(Vector3 vector)
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

        return new Vector3(x, 0, z);
    }

    private void OnDestroy()
    {
        _handler.RegisterUnitForStatUpdates(_baseStats,false);
    }
    protected virtual void UnitBinds(bool isEnable)
    {
        if (isEnable)
        {
            GetStats()[StatType.Health].ValueDecreasedEvent += HealthChangedEvent;
            _controller.CombatActionSuccessEvent += (t) => AnimateCombatActivity(t);
        }
        else
        {
            GetStats()[StatType.Health].ValueDecreasedEvent -= HealthChangedEvent;
            _controller.CombatActionSuccessEvent -= (t) => AnimateCombatActivity(t);
        }
    }


}
