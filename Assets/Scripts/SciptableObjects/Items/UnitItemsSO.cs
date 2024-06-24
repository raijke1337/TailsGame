using Arcatech.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Items
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Unit Items Preset", menuName = "Items/Inventory preset", order = 3)]
    public class UnitItemsSO : ScriptableObjectID
    {
        [SerializeField] public List<Equip> Equipment;
        [SerializeField, Space] public List<Item> Inventory;
    }
}