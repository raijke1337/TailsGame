using Arcatech.Units;
using KBCore.Refs;
using UnityEngine;

namespace Arcatech.Items
{
    public class BaseEquippableItemComponent : ValidatedMonoBehaviour
    {
        [SerializeField] protected Transform _spawner;
        public Transform Spawner => _spawner;
    }
}


