using System;
using UnityEngine;


[Serializable, CreateAssetMenu(fileName = "New Backpack Item", menuName = "Items/Backpack Item")]
public class Item : ScriptableObjectID
{
    public Sprite ItemIcon;
    public string DisplayName;
    public string DescriptionContainerID;
    public EquipItemType ItemType;
}