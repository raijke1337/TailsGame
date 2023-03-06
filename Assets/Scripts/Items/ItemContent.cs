using System;
using UnityEngine;

// this is what is held in bag

[Serializable, CreateAssetMenu(fileName = "New Item content", menuName = "Items/ItemContent")]
public class ItemContent : ScriptableObjectID
{
    public Sprite ItemIcon;
    public string DisplayName;
    public EquipItemType ItemType;
    public string SkillString;
}