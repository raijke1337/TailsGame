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

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerController = _controller as InputsPlayer;     
        _visualController = GetComponent<VisualsController>();  
        ToggleCamera(true);

        float maxHP = _baseStats.GetBaseStats[StatType.Health].GetMax();
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


    


    public override void ApplyEffect(TriggeredEffect eff)
        // shield damage reduction logic
    {
        switch (eff.StatID)
        {
            case StatType.Health:
                _baseStats.AddTriggeredEffect(_playerController.GetShieldController.ProcessHealthChange(eff));
                break;
            case StatType.HealthRegen:
                _baseStats.AddTriggeredEffect(eff);
                break;
            case StatType.Heat:
                _baseStats.AddTriggeredEffect(eff);
                break;
            case StatType.HeatRegen:
                _baseStats.AddTriggeredEffect(eff);
                break;
            case StatType.MoveSpeed:
                _baseStats.AddTriggeredEffect(eff);
                break;
        }
    }


    #region dodge

    private Coroutine _dodgeCor;
    // stop the dodge like this
    private void OnCollisionEnter(Collision collision)
    {
        if (_dodgeCor != null && collision.gameObject.tag != "Ground")
        {
            _playerController.IsControlsBusy = false;
            StopCoroutine(_dodgeCor);
        }
    }
    protected override void DodgeAction()
    {
        _dodgeCor = StartCoroutine(DodgingMovement());
    }
    private IEnumerator DodgingMovement()
    {
        var stats = _playerController.GetDodgeController.GetDodgeStats();
        _playerController.IsControlsBusy = true;

        Vector3 start = transform.position;
        //Vector3 end = start + transform.forward * stats[DodgeStatType.Range].GetCurrent();
        Vector3 end = start + _controller.MoveDirection * stats[DodgeStatType.Range].GetCurrent();

        float p = 0f;
        while (p <= 1f)
        {
            p += Time.deltaTime * stats[DodgeStatType.Speed].GetCurrent();
            transform.position = Vector3.Lerp(start, end, p);
            yield return null;
        }
        _playerController.IsControlsBusy = false;
        yield return null;
    }


    #endregion

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

    protected override void HealthChangedEvent(float value,float prevvalue)
    {
        base.HealthChangedEvent(value,prevvalue);
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
            _playerController.ChangeLayerEvent += ChangeAnimatorLayer;
        }
        else
        {
            _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;
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
            * GetStats()[StatType.MoveSpeed].GetCurrent();
    }
}
