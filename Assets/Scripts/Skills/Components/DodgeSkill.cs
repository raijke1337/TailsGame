using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using UnityEngine;

namespace Arcatech.Skills
{
    public class DodgeSkill : SelfSkill
    {
        protected override void CallExpiry()
        {
            base.CallExpiry();
        }
        protected override void CallTriggerHit(BaseUnit target)
        {
            //base.CallTriggerHit(target); 
            // no triggers on boost skll
        }
        protected override void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger enter {this} {other.gameObject.name}");
            if (other.CompareTag(Owner.tag)) // probably TODO , but should always hit the user first
            {
                _coll.enabled = false;
                transform.SetParent(other.transform, false);
                StartCoroutine(Movement());
            }

        }
        public override void UpdateInDelta(float delta)
        {

            // base.UpdateInDelta(delta); 
            // nothing here anyway
        }

        private IEnumerator Movement()
        {
            //Debug.Log($"Wheeeeeeee! Booster skill active");
            yield return null;
        }

        public override void AssignValues(SkillObjectForControls cfg)
        {
            base.AssignValues(cfg);
            
        }
        public override void SetupStatsComponent()
        {
            base.SetupStatsComponent();
        }

    }

}