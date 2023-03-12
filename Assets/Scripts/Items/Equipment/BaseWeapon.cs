using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseWeapon : EquipmentBase, IWeapon
{

    public bool IsSetup { get; private set; } = false;
    protected StatValueContainer WeaponUses;

    protected float InternalCooldown;
    protected float _currentCooldown;

    public float GetRemainingUses { get { return WeaponUses.GetCurrent; } }


    protected bool IsBusy = false;
    protected List<string> _effectsIDs;
    public event SimpleEventsHandler<float> TargetHit; // only for combo
    protected void TargetHitCallback(float val) => TargetHit?.Invoke(val);

    protected virtual void OnEnable()
    {
        _effectsIDs = new List<string>();
    }

    public virtual bool UseWeapon(out string reason)
    {
        reason = "Ok";

        if (_currentCooldown < InternalCooldown)
        {
            reason = "Weapon on cooldown";
            return false;
        }
        else
        {
            _currentCooldown = 0f;
            return true;
        }
    }

    // loaded by weaponcontroller
    public virtual void AddTriggerData(string effectID)
    {
        _effectsIDs.Add(effectID);
    }

    public virtual void SetUpWeapon(BaseWeaponConfig config)
    {
        foreach (string triggerID in config.TriggerIDs)
        {
            AddTriggerData(triggerID);
        }
        WeaponUses = new StatValueContainer(config.Charges);
        InternalCooldown = config.InternalCooldown;
        IsSetup = true;
    }
    public void UpdateInDelta(float deltaTime)
    {
        _currentCooldown += deltaTime;
    }


}

