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

    //add subs to controller success events
    //public SimpleEventsHandler PlayerMeleeAttackSuccessEvent;
    //public SimpleEventsHandler PlayerRangedAttackSuccessEvent;
    //public SimpleEventsHandler PlayerQSuccessEvent;
    //public SimpleEventsHandler PlayerESuccessEvent;
    //public SimpleEventsHandler PlayerRSuccessEvent;
    //public SimpleEventsHandler PlayerDashSuccessEvent;

    private void OnDestroy()
    {
        PlayerBinds(false);
    }
    private void PlayerBinds(bool isRegister = true)
    {
        _playerController.PlayerDashSuccessEvent += AnimateDash;
        _playerController.PlayerESuccessEvent += AnimateE;
        _playerController.PlayerMeleeAttackSuccessEvent += AnimateMelee;
        _playerController.PlayerQSuccessEvent += AnimateQ;
        _playerController.PlayerRangedAttackSuccessEvent += AnimateRanged;
        _playerController.PlayerRSuccessEvent += AnimateR;
    }

    private void AnimateDash()
    {
        Debug.Log("Dash success");
    }
    private void AnimateE()
    {
        Debug.Log("E success");
    }
    private void AnimateMelee()
    {
        Debug.Log("Melee success");
    }
    private void AnimateQ()
    {
        Debug.Log("Q success");
    }
    private void AnimateRanged()
    {
        Debug.Log("Ranged success");
    }
    private void AnimateR()
    {
        Debug.Log("R success");
    }

}
