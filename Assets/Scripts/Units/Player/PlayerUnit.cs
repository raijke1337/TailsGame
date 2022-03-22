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

public class PlayerUnit : BaseUnit
{
    private PlayerUnitController _playerController;
    public PlayerUnitController GetController => _playerController;

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerController = _controller as PlayerUnitController;        
        PlayerBinds();
    }


    private void OnDestroy()
    {
        PlayerBinds(false);
    }
    private void PlayerBinds(bool isRegister = true)
    {
        if (isRegister)
        {
            _playerController.CombatActionSuccessEvent += (t) => AnimateCombatActivity(t);
            _playerController.ChangeLayerEvent += ChangeAnimatorLayer;
            _playerController.DodgeCompletedAnimatingEvent += OnAnimationComplete;
            _playerController.TargetLockedEvent += TargetUpdate;
        }
        else
        {
            _playerController.CombatActionSuccessEvent -= (t) => AnimateCombatActivity(t);
            _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;
            _playerController.DodgeCompletedAnimatingEvent -= OnAnimationComplete;
        }
    }
    

    private void ChangeAnimatorLayer(WeaponType type)
    {
        // 1 is ranged 2 is hammer
        switch (type)
        {
            case WeaponType.Melee:
                _animator.SetLayerWeight(2, 100f);
                _animator.SetLayerWeight(1, 0f);
                break;
            case WeaponType.Ranged:
                _animator.SetLayerWeight(1, 100f);
                _animator.SetLayerWeight(2, 0f);
                break;
        }
    }

    protected override void AnimateCombatActivity(CombatActionType type)
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
                break;
            case CombatActionType.MeleeSpecialQ:
                _animator.SetTrigger("QSpecial");
                break;
            case CombatActionType.RangedSpecialE:
                _animator.SetTrigger("ESpecial");
                break;
            case CombatActionType.ShieldSpecialR:
                _animator.SetTrigger("RSpecial");
                break;
        }
    }

    public void ComboWindowStart()
    {
        _animator.SetBool("AdvancingCombo",true);
    }
    public void ComboWindowEnd()
    {
        _animator.SetBool("AdvancingCombo", false);
    }

    protected override void TargetUpdate(IStatsAvailable unit)
    {
        base.TargetUpdate(unit);
    }
}
