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

[Serializable]
public abstract class ItemBase : MonoBehaviour, IInventoryItem
{
    // this is a backpack item 
    
    [SerializeField] protected ItemContent _itemContent;

    public string GetID => _itemContent.ID;
    public ItemContent GetContents => _itemContent;

}

