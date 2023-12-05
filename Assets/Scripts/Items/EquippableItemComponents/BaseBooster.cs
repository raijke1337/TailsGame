using Arcatech.Triggers;
using UnityEngine;

namespace Arcatech.Items
{
    public class BaseBooster : BaseEquippableItemComponent
    {
        
        public override void UpdateInDelta(float delta)
        {
           //if (_spawnedSkill != null)
           // {
           //     _spawnedSkill.UpdateInDelta(delta);
           // }
        }
        public override float GetNumericValue => base.GetNumericValue;

    }
}