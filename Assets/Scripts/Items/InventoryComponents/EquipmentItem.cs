using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Items
{


    [Serializable]
    public class EquipmentItem : InventoryItem, IHasSkill
    {

        public SerializedStatModConfig[] ItemStats { get; protected set; }

        public EffectsCollection Effects { get; private set; } 
        
        protected BaseEquippableItemComponent _gameItem;

        public SerializedSkillConfiguration GetSkillData { get; protected set; }

        public EquipmentItem (Equip cfg) : base (cfg)
        {
            Debug.Log($"Create equipitem {cfg}");
            _gameItem = GameObject.Instantiate(cfg.ItemPrefab);
            Effects = new EffectsCollection(cfg.Effects);
            ItemStats = cfg.StatMods;

            GetSkillData = cfg.Skill;
            _gameItem.gameObject.SetActive(false);

        }
        public void SetItemEmpty(Transform pos)
        {
            ItemShown = true;

            _gameItem.transform.SetPositionAndRotation(pos.position, pos.rotation);
            _gameItem.transform.SetParent(pos.transform);
        }



        public bool ItemShown
        {
            get { return _gameItem.gameObject.activeSelf; }
            set
            {
                _gameItem.gameObject.SetActive(value);
            }
        }
    }
}