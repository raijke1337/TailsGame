using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public abstract class BaseEquippableItemComponent : MonoBehaviour, IHasOwner
    {
        public BaseUnit Owner { get; set; }
        public abstract void UpdateInDelta(float delta);

        // placeholder - ui takes values from weapions, not skill  controller
        public virtual float GetNumericValue { get => 0; }

    }
}
