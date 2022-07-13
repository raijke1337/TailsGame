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

public abstract class EquipmentBase : ItemBase, IEquippable
{
    public BaseUnit Owner { get; set; }
    public GameObject GetObject() => gameObject;

    public void OnEquip(ItemContent content)
    {
        ItemContents = content;
    }
    protected void Start()
    {
        if (ItemContents == null) Debug.LogError("OnEquip not run for " + this);
    }
}

