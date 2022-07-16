using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitInventoryComponent : MonoBehaviour, IStatsComponentForHandler
{
    [SerializeField] private SerializableDictionaryBase<EquipItemType,ItemContent> Equipments;

    // item receivers
    private List<IUsesItems> _slots = new List<IUsesItems>();

    public bool IsReady => Equipments.Count() > 0;

    public void AddItemUser(IUsesItems slot) => _slots.Add(slot);

    public IEnumerable<ItemContent> GetEquippedItems() => Equipments.Values;
    public ItemContent GetEquippedItem(EquipItemType type) => Equipments[type];



    private void OnValidate()
    {
        foreach (var item in Equipments.Values)
        {
            item.ContentItem.ItemContents = item; // yikes
        }
    }
    public void UpdateInDelta(float deltaTime) { }

    public void SetupStatsComponent()
    {
        foreach (var user in _slots)
        {
            foreach (var item in Equipments.Values)
            {
                user.LoadItem(item.ContentItem);
                // todo
            }
        }
    }

    public void EquipItem(ItemContent content,bool isEquip)
    {
        if (isEquip) Equipments[content.ItemType] = content;
        else Equipments[content.ItemType] = null;
        SetupStatsComponent();
    }



}




