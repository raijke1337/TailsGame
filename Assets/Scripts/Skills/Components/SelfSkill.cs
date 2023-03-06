using UnityEngine;

public class SelfSkill : BaseSkill
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (Owner == null) return; // test

        if (other.CompareTag(Owner.tag)) base.OnTriggerEnter(other);
    }
}

