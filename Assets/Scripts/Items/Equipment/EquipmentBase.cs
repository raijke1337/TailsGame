using UnityEngine;

public abstract class EquipmentBase : ItemBase, IEquippable
{
    public BaseUnit Owner { get; set; }

    public EquipmentBase GetEquipmentBase()
    {
        return this;
    }

    public GameObject GetObject() => gameObject;


}

