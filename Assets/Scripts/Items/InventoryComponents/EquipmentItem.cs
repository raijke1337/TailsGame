using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Items
{
    [Serializable]
    public class EquipmentItem : InventoryItem
    {
        public BaseUnit Owner { get; set; }
        public string SkillString { get; }


        [SerializeField] protected BaseEquippableItemComponent _prefab;
        protected BaseEquippableItemComponent _instantiated;
        public BaseEquippableItemComponent GetInstantiatedPrefab 
        {
            get
            {
                if (_instantiated == null)
                {
                    _instantiated = GameObject.Instantiate(_prefab);                    
                }
                return _instantiated;
            }          
        }
        public bool IsInstantiated
        {
            get
            {
                return (_instantiated!= null && _instantiated.isActiveAndEnabled); // mighht get nullref
            }
            set
            {
                if (_instantiated != null)
                {
                    _instantiated.gameObject.SetActive(value);
                }
            }
        }

        public EquipmentItem(Equip config) : base(config)
        {
            SkillString = config.SkillString; _prefab = config.Item;
        }

        public virtual void UpdateInDelta(float delta)
        {
            if (_instantiated!= null)
            {
                _instantiated.UpdateInDelta(delta);
            }
        }

    }

}