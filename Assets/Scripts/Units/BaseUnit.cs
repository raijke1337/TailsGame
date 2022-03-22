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

public abstract class BaseUnit : MonoBehaviour, IStatsAvailable
{
    [SerializeField]
    protected BaseStatsController _baseStats;
    protected Animator _animator;
    protected Rigidbody _rigidbody;
    protected BaseUnitController _controller;

    [SerializeField] protected string _name;

    public BaseUnit Target { get; set; }
    // nyi todo

    public string GetName() => _name;
    public Rigidbody GetRigidBody => _rigidbody;
    public IReadOnlyDictionary<StatType,StatValueContainer> GetStats() => _baseStats.GetBaseStats;
    [Inject]
    protected StatsUpdatesHandler _handler;

    //
    public event SimpleEventsHandler<IStatsAvailable> UnitDiedEvent;

    public void ApplyEffect(TriggeredEffect eff)
    {
        _baseStats.AddTriggeredEffect(eff);
    }

    protected virtual void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<BaseUnitController>();
        _rigidbody = GetComponent<Rigidbody>();
        GetStats()[StatType.Health].ValueDecreasedEvent += HealthChangedEvent;


        _handler.RegisterUnitForStatUpdates(_baseStats);
    }
    protected virtual void Update()
    {
        AnimateAndPerformMovement();
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
            Debug.Log($"{name}: 'Ooof'");
        }
    }

    //sets animator values and moves transfrom
    protected void AnimateAndPerformMovement()
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
            transform.position += movement * Time.deltaTime * GetStats()[StatType.MoveSpeed].GetCurrent();
            CalcAnimVector(movement);
        }
    }
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
        private Vector3 CalcAnimVector(Vector3 vector)
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

    protected virtual void TargetUpdate(IStatsAvailable unit)
    {
        Target = unit as BaseUnit;
    }
    

}