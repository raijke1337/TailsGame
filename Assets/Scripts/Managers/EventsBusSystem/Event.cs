using Arcatech.Units;
using UnityEngine;

namespace Arcatech.EventBus
{
    public interface IEvent { }

    public struct DrawDamageEvent : IEvent
    {
        public DrawDamageEvent (float num, BaseUnit u)
        {
            Damage = num; Unit = u;
        }
        public float Damage;
        public BaseUnit Unit;
    }


}

