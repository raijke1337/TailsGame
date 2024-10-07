using Arcatech.EventBus;
using CartoonFX;
using UnityEngine;

namespace Arcatech.Effects
{
    public struct VFXRequest : IEvent
    {
        public VFXRequest(CFXR_Effect effect, Transform place, Transform parent = null)
        {
            Effect = effect;
            Parent = parent;
            Place = place;
        }

        public CFXR_Effect Effect { get; }
        public Transform Parent { get; }
        public Transform Place { get; }
    }
}