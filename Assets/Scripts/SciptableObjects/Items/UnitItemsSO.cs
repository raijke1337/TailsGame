using Arcatech.Items;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Unit Items Preset", menuName = "Equipments/Inventory", order = 3)]
public class UnitItemsSO : ScriptableObjectID
{
    [SerializeField] public List<Equip> Equipment;
    [SerializeField, Space] public List<Item> Inventory;
    [SerializeField, Space] public List<Item> Drops;
}
