using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Items
{
    [Serializable]
    public class EquipmentItem : InventoryItem
    {
        public string SkillString { get; }


        [SerializeField] protected BaseEquippableItemComponent _prefab;
        [SerializeField] protected AudioComponentBase _sounds;

        public AudioComponentBase GetSounds => _sounds;


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
                SkillString = e.SkillString;
                _prefab = e.Item;
            }
        }
        public void StopGameItem()
        {
            GameObject.Destroy(_instantiated);

        }

    }

}