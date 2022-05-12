using System.Collections.Generic;
using UnityEngine;
using Zenject;
public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    public string ID;

    private WeaponType WeapType;

    protected int MaxCharges;
    protected int _currentCharges;
    protected int ComboVal;
    public int GetAmmo { get { return _currentCharges; } }

    protected string SkillID;
    public string GetRelatedSkillID() => SkillID;

    protected bool IsBusy = false;

    [Inject] protected PlayerUnit _player;
    protected List<string> _effectsIDs;

    public event SimpleEventsHandler<float> TargetHit;
    protected void TargetHitCallback(float val) => TargetHit?.Invoke(val);

    protected virtual void OnEnable()
    {
        _effectsIDs = new List<string>();
    }

    public abstract bool UseWeapon();

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
    }

}

