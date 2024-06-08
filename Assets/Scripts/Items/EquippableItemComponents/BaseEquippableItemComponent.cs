using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class BaseEquippableItemComponent : MonoBehaviour, IHasOwner
    {
        public BaseUnit Owner { get; set; }

    }
}
