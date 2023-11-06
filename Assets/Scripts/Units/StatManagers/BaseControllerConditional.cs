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
        protected EquipmentItem _item;
        public SimpleEventsHandler<EquipmentItem> EquippedItemChangedEvent;


        public virtual void LoadItem(EquipmentItem item, out string skill)
        {
            item.Owner = Owner;
            skill = item.SkillString;
        }
        public BaseEquippableItemComponent CurrentlyEquippedItemObject
        {
            get => _item.GetInstantiatedPrefab;
        }

        public EquipmentItem CurrentlyEquippedItem 
        { get
            {
                return _item;
            }
            protected set
            {
                _item = value;
                if (_item != null)
                {
                    EquippedItemChangedEvent?.Invoke(CurrentlyEquippedItem);
                    IsReady = true;
                }
            }
        }
        
    }
}
