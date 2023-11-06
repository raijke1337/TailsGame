using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Equip Item", menuName = "Items/Equip Item")]
    public class Equip : Item
    {
        //public Sprite ItemIcon;
        //public string DisplayName;
        //public string DescriptionContainerID;
        //public EquipItemType ItemType;

        public string SkillString;
        public BaseEquippableItemComponent Item;

    }
}