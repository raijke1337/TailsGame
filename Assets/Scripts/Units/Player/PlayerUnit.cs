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
    private VisualsController _visualController;
    private float[] _visualStagesHP;
    private int _currentVisualStageIndex;

    public event SimpleEventsHandler<GameMenuType> ToggleMenuEvent;

    protected override void OnEnable()
    {
        base.OnEnable();
        ToggleCamera(true);
        _visualController = GetComponent<VisualsController>();

        var stats = _controller.GetStatsController;

        float maxHP = stats.GetBaseStats[BaseStatType.Health].GetMax;
        stats.GetBaseStats[BaseStatType.Health].ValueChangedEvent += ChangeVisualStage;

        int stages = _visualController.StagesTotal;
        _visualStagesHP = new float[stages];
        var coef = maxHP / stages;
        for (int i = 0; i < stages; i++)
        {
            _visualStagesHP[i] = maxHP;
            maxHP -= coef;
        }
        _currentVisualStageIndex = 0;
    }    


    private void ChangeAnimatorLayer(EquipItemType type)
    {     
        // 1  2 is ranged 3  is melee
        switch (type)
        {
            case EquipItemType.MeleeWeap:
                _animator.SetLayerWeight(0, 0f);
                _animator.SetLayerWeight(1, 0f);
                _animator.SetLayerWeight(2, 100f);
                break;
            case EquipItemType.RangedWeap:
                _animator.SetLayerWeight(0, 100f);
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
        _playerController.ComboGained = false;
    }

    private void ChangeVisualStage(float value,float prevvalue)
    {
        int newIndex = _currentVisualStageIndex + 1;
        if (newIndex >= _visualStagesHP.Count()) return;
        if (value <= _visualStagesHP[newIndex])
        {
            _currentVisualStageIndex++;
            _visualController.AdvanceMaterialStage();
        }
    }

    protected override void UnitBinds(bool isEnable)
    {
        base.UnitBinds(isEnable);

        if (isEnable)
        {
            _playerController = _controller as InputsPlayer;
            _playerController.ChangeLayerEvent += ChangeAnimatorLayer;
            _playerController.MenuToggleRequest += (t) => ToggleMenuEvent?.Invoke(t);
        }
        else
        {
            _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;
            _playerController.MenuToggleRequest -= (t) => ToggleMenuEvent?.Invoke(t);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PlayerMovement(_playerController.MoveDirection);
    }


    //private Vector3 currVelocity;
    //[SerializeField,Tooltip("How quickly the inertia of movement fades")] private float massDamp = 3f;


    private void PlayerMovement(Vector3 desiredDir)
    {
        if (_controller.IsControlsBusy) return;
        // DO NOT FIX WHAT ISNT BROKEN //

        //if (desiredDir == Vector3.zero)
        //{
        //    if (currVelocity.sqrMagnitude < 0.1f) currVelocity = Vector3.zero;
        //    else currVelocity = Vector3.Lerp(currVelocity, Vector3.zero, Time.deltaTime * massDamp);
        //}
        //else currVelocity = desiredDir;
        //transform.position += GetStats()[StatType.MoveSpeed].GetCurrent() * Time.deltaTime * currVelocity;
        // too slide-y

        // also good enough
        transform.position += Time.deltaTime * desiredDir
            * GetStats()[BaseStatType.MoveSpeed].GetCurrent;
    }

    public void SetInfoPanel(TargetUnitPanel panel) => _playerController.TargetPanel = panel;
}
