using UnityEngine;
namespace Arcatech.Skills
{
    public class SelfSkill : BaseSkill
    {
        public override void UpdateInDelta(float deltaTime)
        {

        }

        protected override void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger enter {this} {other.gameObject.name}");
            if (Owner == null) return; // test

            if (other.CompareTag(Owner.tag)) base.OnTriggerEnter(other);
        }
    }

}