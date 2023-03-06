using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggerComponent : BaseTrigger
{
    public BaseUnit Owner { get; set; }
    protected List<string> TriggerEffectIDs;
    public SimpleEventsHandler HitSuccessEvent;

    public void ToggleCollider (bool isEnable)
    {
        if (_coll == null) _coll = GetComponent<Collider>();
        _coll.enabled = isEnable;
    }
    public void SetTriggerIDS(IEnumerable<string> ids)
    {
        TriggerEffectIDs = new List<string>();
        foreach (string id in ids) { TriggerEffectIDs.Add(id); }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponent<BaseUnit>();
        if (comp == null) return;
        HitSuccessEvent?.Invoke();
        foreach (var id in TriggerEffectIDs)
        {
            TriggerCallback(id, comp, Owner);
        }
    }
}

