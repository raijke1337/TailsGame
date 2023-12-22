using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Items
{
    [Serializable]
    public class EquipmentItem : InventoryItem
    {

        public SkillControlSettingsSO Skill { get; }
        public EffectsCollection Effects { get; }

        [SerializeField] protected BaseEquippableItemComponent _prefab;

        protected BaseEquippableItemComponent _instantiated;


        public EquipmentItem(Item cfg, BaseUnit ow) : base(cfg, ow)
        {
            if (cfg is Equip e)
            {
                Skill = e.Skill;
                _prefab = e.Item;
                Effects = e.Effects;
            }
        }


        public BaseEquippableItemComponent GetInstantiatedPrefab()
        {
            if (_instantiated == null)
            {
                GenerateObject();
            }
            if (!_instantiated.gameObject.activeSelf)
            {
                _instantiated.gameObject.SetActive(true);
            }
            return _instantiated;
        }
        protected virtual void GenerateObject()
        {
            _instantiated = GameObject.Instantiate(_prefab);
            _instantiated.Owner = Owner;

        }




        public void SetItemEmpty(Transform parent)
        {
            if (_instantiated == null)
            {
                GenerateObject();
            }
            _instantiated.transform.SetParent(parent, false);
        }
        public void OnEquip()
        {
            if (_instantiated != null) _instantiated.gameObject.SetActive(true);

        }
        public void OnUnequip()
        {
            if (_instantiated != null) _instantiated.gameObject.SetActive(false);
        }

        public virtual bool TryUseItem()
        {
            if (true) _instantiated.OnItemUse();
            return true;
        }

        public virtual void DoUpdates(float d)
        { }

    }

}