using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public abstract class BaseEquippableItemComponent : MonoBehaviour, IHasOwner
    {
        public BaseUnit Owner { get; set; }
        public abstract void UpdateInDelta(float delta);
        /// <summary>
        /// ammo, energy cost
        /// </summary>
        /// <returns>int</returns>
        public abstract int GetNumericValue { get; }

    }
}
