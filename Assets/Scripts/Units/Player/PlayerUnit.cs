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

    protected override void Start()
    {
        base.Start();
        _playerController = _controller as PlayerUnitController;
        PlayerBinds();
    }

    protected override void Update()
    {
        AnimateAndPerformMovement();
    }

    private void OnDestroy()
    {
        PlayerBinds(false);
    }
    private void PlayerBinds(bool isRegister = true)
    {
        if (isRegister)
        {
            _playerController.PlayerDashSuccessEvent += AnimateDash;
            _playerController.PlayerESuccessEvent += AnimateE;
            _playerController.PlayerMeleeAttackSuccessEvent += AnimateMelee;
            _playerController.PlayerQSuccessEvent += AnimateQ;
            _playerController.PlayerRangedAttackSuccessEvent += AnimateRanged;
            _playerController.PlayerRSuccessEvent += AnimateR;
            _playerController.ChangeLayerEvent += ChangeAnimatorLayer;
        }
        else
        {
            _playerController.PlayerDashSuccessEvent -= AnimateDash;
            _playerController.PlayerESuccessEvent -= AnimateE;
            _playerController.PlayerMeleeAttackSuccessEvent -= AnimateMelee;
            _playerController.PlayerQSuccessEvent -= AnimateQ;
            _playerController.PlayerRangedAttackSuccessEvent -= AnimateRanged;
            _playerController.PlayerRSuccessEvent -= AnimateR;
            _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;

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

    private void AnimateDash()
    {
        _animator.SetTrigger("Dodge");
    }
    private void AnimateE()
    {
        _animator.SetTrigger("ESpecial");
    }   

    protected override void AnimateMelee()
    {
        base.AnimateMelee();
    }
    private void AnimateQ()
    {
        _animator.SetTrigger("QSpecial");
    }
    protected override void AnimateRanged()
    {
        base.AnimateRanged();        
    }
    private void AnimateR()
    {
        _animator.SetTrigger("RSpecial");
    }

}
