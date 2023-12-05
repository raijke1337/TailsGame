using Arcatech.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Arcatech.Units
{
    public abstract class BaseControllerConditional : BaseController, IHasOwner
    {
        public BaseControllerConditional(ItemEmpties empties, BaseUnit _ow) : base (_ow)
        {
            Empties = empties;
        }
        public ItemEmpties Empties { get; }

        #region public
        protected Dictionary<EquipItemType, EquipmentItem> _equipment = new Dictionary<EquipItemType, EquipmentItem>();

        public virtual EquipmentItem[] GetCurrentEquipped { get => _equipment.Values.ToArray(); }

        public void LoadItem(EquipmentItem item, out EquipmentItem removing)
        {
            
            item.Owner = Owner;

            OnItemAssign(item, out removing);

            //Debug.Log($"Loaded item {item.GetDisplayName} for {Owner.GetFullName}");
            if (removing != null)
            {
              //  Debug.Log($"Removed {removing.GetDisplayName}");
            }

            StateChangeCallback(IsReady, this);
        }
        public virtual EquipmentItem RemoveItem(EquipItemType type)
        {
            var e = _equipment[type];
            _equipment.Remove(type);
            IsReady = false;
            return e;
        }

        #endregion

        protected virtual void OnItemAssign(EquipmentItem item, out EquipmentItem replacing)
        {

            replacing = null;
            

            if (_equipment.TryGetValue(item.ItemType, out EquipmentItem val))
            {
                replacing = val;
                RemoveItem(replacing.ItemType);
            }
            IsReady = true;
            var i = _equipment[item.ItemType] = item;
            FinishItemConfig(i);
            InstantiateItem(i);
        }

        protected abstract void FinishItemConfig(EquipmentItem item);
        protected abstract void InstantiateItem(EquipmentItem i);

    }
}
