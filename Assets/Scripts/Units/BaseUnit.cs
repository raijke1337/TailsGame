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
    [SerializeField]
    protected BaseStatsController _baseStats;
    protected Animator _animator;

    protected BaseUnitController _controller;

    // use to disable controls for attack animations, stuns, skills, etc
    public bool IsControlBusy { get; set; }

    public IReadOnlyDictionary<StatType,StatValueContainer> GetStats => _baseStats.GetBaseStats;
    [Inject]
    protected StatsUpdatesHandler _handler;

    public void ApplyEffect(TriggeredEffect eff)
    {
        _baseStats.AddTriggeredEffect(eff);
    }

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<BaseUnitController>();
    }
    protected virtual void Update()
    {
        AnimateAndPerformMovement();
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
            transform.position += movement * Time.deltaTime * GetStats[StatType.MoveSpeed].GetCurrent();
            CalcAnimVector(movement);
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

    private void OnEnable()
    {
        _handler.RegisterUnitForStatUpdates(_baseStats);
    }

    private void OnDestroy()
    {
        _handler.RegisterUnitForStatUpdates(_baseStats,false);
    }


    protected virtual void AnimateMelee()
    {
        _animator.SetTrigger("MeleeAttack");
    }
    protected virtual void AnimateRanged()
    {
        _animator.SetTrigger("RangedAttack");
    }
}

