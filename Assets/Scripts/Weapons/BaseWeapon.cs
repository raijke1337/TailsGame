using System.Collections.Generic;
using UnityEngine;
using Zenject;
public abstract class BaseWeapon : BaseItem, IWeapon
{
    private WeaponType WeapType;

    protected int MaxCharges;
    protected int _currentCharges;
    protected int ComboVal;
    protected float InternalCooldown;

    protected float _currentCooldown;
    public int GetAmmo { get { return _currentCharges; } }

    public BaseUnit Owner { get ; set ; }

    protected string SkillID;
    public string GetRelatedSkillID() => SkillID;

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

    public GameObject GetObject() => gameObject;

    public void SetUpWeapon(BaseWeaponConfig config)
    {
        WeapType = config.WType;
        foreach (string triggerID in config.TriggerIDs)
        {
            AddTriggerData(triggerID);
        }
        MaxCharges = config._charges;
        SkillID = config.SkillID;
        ComboVal = config.ComboValue;
        InternalCooldown = config.InternalCooldown;
    }
    public void UpdateInDelta(float deltaTime)
    {
        _currentCooldown += deltaTime;
    }

}

