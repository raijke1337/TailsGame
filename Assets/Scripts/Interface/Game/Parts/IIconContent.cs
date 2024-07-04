using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.UI
{
    public interface IIconContent
    {
        public Sprite Icon { get; }
        public float CurrentNumber { get; }
        public float MaxNumber { get; }

    }
}