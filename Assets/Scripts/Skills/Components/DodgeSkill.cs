using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

public class DodgeSkill : SelfSkill
{
    protected override void CallExpiry()
    {
        base.CallExpiry();
    }
    protected override void CallTriggerHit(BaseUnit target)
    {
        base.CallTriggerHit(target);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

}

