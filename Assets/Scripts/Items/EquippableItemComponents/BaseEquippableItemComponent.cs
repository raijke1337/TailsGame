using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class BaseEquippableItemComponent : MonoBehaviour
    {
        protected BaseUnit _owner;
        public BaseEquippableItemComponent SetOwner (BaseUnit owner)
        {
            _owner = owner;
            return this;
        }

    }
}


