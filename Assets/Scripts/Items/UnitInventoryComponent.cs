using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UnitInventoryComponent
{
    private Dictionary<EquipItemType, IEquippable> Equipments;

    ItemsEquipmentsHandler Handler;

    public UnitInventoryComponent (ItemsEquipmentsHandler handler)
    {
        Handler = handler;
        Equipments = new Dictionary<EquipItemType, IEquippable> ();
    }
    public IEquippable EquipItem(string ID)
    {
        var item = Handler.GetItemByID(ID);
        if (item is IEquippable)
        {
            var i = item as IEquippable;
            Equipments[i.GetContents.ItemType] = i;
            return i;
        }
        else
        {
            Debug.LogWarning($"Tried to equip {ID}");
            return null;
        }
    }

}




