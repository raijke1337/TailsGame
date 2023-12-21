using System;
using UnityEngine;


[Serializable, CreateAssetMenu(fileName = "New Backpack Item", menuName = "Items/Backpack Item")]
public class Item : ScriptableObjectID
{
    public TextContainerSO Description;
    public EquipItemType ItemType;
}