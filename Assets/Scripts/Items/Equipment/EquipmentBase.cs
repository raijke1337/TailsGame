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

    public EquipmentBase GetEquipmentBase()
    {
        return this;
    }

    public GameObject GetObject() => gameObject;
     

}

