using Arcatech.Texts;
using System;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Backpack Item", menuName = "Items/Backpack Item")]
    public class ItemSO : ScriptableObjectID
    {
        public ExtendedText Description;
        public int MaxStack = 1;

        public EquipmentType Type;
    }
}