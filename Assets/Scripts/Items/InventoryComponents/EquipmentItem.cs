using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using CartoonFX;
using System;
using UnityEngine;
namespace Arcatech.Items
{
    [Serializable]
    public class EquipmentItem : InventoryItem, IHasEffects
    {
        //public string SkillString { get; }
        public SkillControlSettingsSO ItemSkillConfig { get; }
        protected EffectsCollection _effects;



        [SerializeField] protected BaseEquippableItemComponent _prefab;

        protected BaseEquippableItemComponent _instantiated;
        public BaseEquippableItemComponent GetInstantiatedPrefab 
        {
            get
            {
                if (_instantiated == null)
                {
                    _instantiated = GameObject.Instantiate(_prefab);
                    _instantiated.Owner = Owner;
                }
                if (!_instantiated.gameObject.activeSelf)
                {
                    _instantiated.gameObject.SetActive(true);
                }
                return _instantiated;
            }          
        }
        public EquipmentItem(Item cfg, BaseUnit ow) : base (cfg,ow)
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
        public void OnUnequip()
        {
            if (_instantiated != null) _instantiated.gameObject.SetActive(false);
        }

        public AudioClip GetAudio(EffectMoment type)
        {
            try
            {
                return _effects.Sounds[type];
            }
            catch
            {
                //Debug.LogWarning($"Audio not set for {this.GetDisplayName} for {type}");
                return null;
            }
        }

        public CFXR_Effect GetParticle(EffectMoment type)
        {
            try
            {
                return _effects.Effects[type];
            }

            catch
            {
                //Debug.LogWarning($"Particle not set for {this.GetDisplayName} for {type}");
                return null;
            }
        }
    }

}