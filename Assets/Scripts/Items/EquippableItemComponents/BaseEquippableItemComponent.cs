using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public abstract class BaseEquippableItemComponent : MonoBehaviour, IHasOwner
    {
        public BaseUnit Owner { get; set; }
        public abstract void OnItemUse();

    }
}
