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

        public SkillControlSettingsSO ItemSkillConfig { get; }
        protected EffectsCollection _effects;
        public EffectsCollection GetEffects => _effects;

        [SerializeField] protected BaseEquippableItemComponent _prefab;

        protected BaseEquippableItemComponent _instantiated;
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
        public void SetItemEmpty(Transform parent)
        {
            if (_instantiated == null)
            {
                GenerateObject();
            }
            _instantiated.transform.SetParent(parent, false);
        }


        private void GenerateObject()
        {
            _instantiated = GameObject.Instantiate(_prefab);
            _instantiated.Owner = Owner;
            if (_instantiated is BaseWeapon weap)
            {
                weap.TriggerEvent += OnWeapTriggerEvent;
            }
        }


        public EquipmentItem(Item cfg, BaseUnit ow) : base(cfg, ow)
        {
            if (cfg is Equip e)
            {
                ItemSkillConfig = e.Skill;
                _prefab = e.Item;
                _effects = e.Effects;
            }
        }

        public void OnEquip()
        {
            if (_instantiated != null) _instantiated.gameObject.SetActive(true);

        }

        public event TriggerEvent PrefabTriggerHitSomething;
        private void OnWeapTriggerEvent(BaseUnit target, BaseUnit source, bool isEnter, BaseStatTriggerConfig cfg)
        {
            //Debug.Log($"On weapon trigger event in {GetDisplayName}");
            if (isEnter)
            {
                PrefabTriggerHitSomething?.Invoke(target, source, isEnter, cfg);
            }
        }

        public void OnUnequip()
        {
            if (_instantiated != null) _instantiated.gameObject.SetActive(false);
        }

    }

}