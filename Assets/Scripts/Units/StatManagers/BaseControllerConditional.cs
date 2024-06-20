using Arcatech.Items;
using System.Collections.Generic;
using System.Linq;

namespace Arcatech.Units
{
    public abstract class BaseControllerConditional : BaseController, IHasOwner
    {
        public BaseControllerConditional(ItemEmpties empties, BaseUnit owner) : base(owner)
        {
            Empties = empties;
        }
        public ItemEmpties Empties { get; }

        #region public
        protected Dictionary<EquipmentType, EquipmentItem> _items = new Dictionary<EquipmentType, EquipmentItem>();

        public void LoadItem(EquipmentItem item, out EquipmentItem removing)
        {

            item.Owner = Owner;
            OnItemAssign(item, out removing);
            if (item is WeaponItem w)
            {
                w.PrefabTriggerHitSomething += TriggerEventCallback;
            }
           
            StateChangeCallback(IsReady, this);
        }
        public virtual bool TryRemoveItem(EquipmentType type, out EquipmentItem removed)
        {
            bool success = false;
            removed = null;
            if (_items.ContainsKey(type))
            {
                removed = _items[type];
                _items.Remove(type);
                IsReady = false;
                if (removed is WeaponItem w)
                {
                    w.PrefabTriggerHitSomething -= TriggerEventCallback;
                }
                success = true;
            }

            return success;
        }

        #endregion

        protected virtual void OnItemAssign(EquipmentItem item, out EquipmentItem replacing)
        {

            replacing = null;
            if (_items.TryGetValue(item.ItemType, out EquipmentItem val))
            {
                replacing = val;
                TryRemoveItem(replacing.ItemType, out replacing);
            }
            IsReady = true;
            var i = _items[item.ItemType] = item;
            FinishItemConfig(i);
            InstantiateItem(i);
        }

        protected abstract void FinishItemConfig(EquipmentItem item);
        protected abstract void InstantiateItem(EquipmentItem i);

    }
}
