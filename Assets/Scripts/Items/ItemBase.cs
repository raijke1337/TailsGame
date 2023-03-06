using System;
using UnityEngine;

[Serializable]
public abstract class ItemBase : MonoBehaviour, IInventoryItem
{
    // this is a backpack item 

    [SerializeField] protected ItemContent _itemContent;

    public string GetID => _itemContent.ID;
    public ItemContent GetContents => _itemContent;

}

