using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class BaseEquippableItemComponent : MonoBehaviour, IHasOwner
    {
        public BaseUnit Owner { get; set; }
        public string ID { get; set; }
        public virtual void UpdateInDelta(float delta)
        { }

    }
}
