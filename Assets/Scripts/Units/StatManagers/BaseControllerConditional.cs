using Arcatech.Items;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.Progress;

namespace Arcatech.Units
{
    public abstract class BaseControllerConditional : BaseController, IHasOwner
    {
        public BaseControllerConditional(ItemEmpties empties, BaseUnit _ow) : base(_ow)
        {
            Empties = empties;
        }
        public ItemEmpties Empties { get; }

        #region public
        protected Dictionary<EquipItemType, EquipmentItem> _equipment = new Dictionary<EquipItemType, EquipmentItem>();

        public void LoadItem(EquipmentItem item, out EquipmentItem removing)
        {

            item.Owner = Owner;

            OnItemAssign(item, out removing);

            //Debug.Log($"Loaded item {item.GetDisplayName} for {Owner.GetFullName}");
            if (removing != null)
            {
                //  Debug.Log($"Removed {removing.GetDisplayName}");
            }
            if (item is WeaponItem w)
            {
                w.PrefabTriggerHitSomething += TriggerEventCallback;
            }
           
            StateChangeCallback(IsReady, this);
        }
        public virtual bool TryRemoveItem(EquipItemType type, out EquipmentItem removed)
        {
            bool success = false;
            removed = null;
            if (_equipment.ContainsKey(type))
            {
                removed = _equipment[type];
                _equipment.Remove(type);
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


            if (_equipment.TryGetValue(item.ItemType, out EquipmentItem val))
            {
                replacing = val;
                TryRemoveItem(replacing.ItemType, out replacing);
            }
            IsReady = true;
            var i = _equipment[item.ItemType] = item;
            FinishItemConfig(i);
            InstantiateItem(i);
        }

        protected abstract void FinishItemConfig(EquipmentItem item);
        protected abstract void InstantiateItem(EquipmentItem i);
        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);
            foreach (var e in _equipment.Values)
            {
                e.DoUpdates(deltaTime);
            }
        }

    }
}
