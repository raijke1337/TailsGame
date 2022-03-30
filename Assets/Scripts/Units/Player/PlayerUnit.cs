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
    private InputsPlayer _playerController;

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerController = _controller as InputsPlayer; 
        
        ToggleCamera(true);
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

    public void ComboWindowStart()
    {
        _animator.SetBool("AdvancingCombo",true);
    }
    public void ComboWindowEnd()
    {
        _animator.SetBool("AdvancingCombo", false);
    }

    protected override void HealthChangedEvent(float value)
    {
        base.HealthChangedEvent(value);
    }

    protected override void UnitBinds(bool isEnable)
    {
        base.UnitBinds(isEnable);

        if (isEnable)
        {
            _playerController.ChangeLayerEvent += ChangeAnimatorLayer;
        }
        else
        {
            _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;
        }
    }

    protected override void Update()
    {
        base.Update();
        PlayerMovement(_playerController.MoveDirection);
    }

    private void PlayerMovement(Vector3 desiredDir)
    {
        transform.position += Time.deltaTime * desiredDir * GetStats()[StatType.MoveSpeed].GetCurrent();
    }

}
