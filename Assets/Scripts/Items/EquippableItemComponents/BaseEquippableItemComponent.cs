using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class BaseEquippableItemComponent : MonoBehaviour, IHasOwner
    {
        public BaseUnit Owner { get; set; }
        public virtual void UpdateInDelta(float delta)
        { }
        /// <summary>
        /// ammo, energy cost
        /// </summary>
        /// <returns>int</returns>
        public virtual int GetNumericValue()
        {
            return 0;
        }

    }
}
