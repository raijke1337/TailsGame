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

public abstract class ItemBase : MonoBehaviour, IInventoryItem
{
    public ItemContent ItemContents { get; set; }
    public string GetID => ItemContents.ID;
}

