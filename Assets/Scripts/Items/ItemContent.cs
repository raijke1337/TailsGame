using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// this is what is held in bag

[Serializable, CreateAssetMenu(fileName = "New Item content", menuName = "Items/ItemContent")]
public class ItemContent : ScriptableObjectID
{
    public Sprite ItemIcon;
    public string DisplayName;
    public EquipItemType ItemType;
    public string SkillString;
    public EquipmentBase ContentItem;
}