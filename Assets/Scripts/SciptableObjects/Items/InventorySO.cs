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

[CreateAssetMenu(fileName = "New Inventory Preset", menuName = "Equipments/Inventory", order = 3)]
public class InventorySO : ScriptableObjectID
{
    public string[] itemIDs;
}

