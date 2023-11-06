using Arcatech.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class WeaponController : BaseControllerConditional
    {

        public WeaponController(ItemEmpties ie) => Empties = ie;


        private Dictionary<EquipItemType, EquipmentItem> _availableWeap = new Dictionary<EquipItemType, EquipmentItem>();
        private Dictionary<EquipmentItem, BaseWeaponConfig> _cachedConfigs = new Dictionary<EquipmentItem, BaseWeaponConfig>();

        public ItemEmpties Empties { get; }


        public WeaponEvents<EquipItemType> SwitchAnimationLayersEvent; // also used for layers switch in playerunit

        public void SwitchModels(EquipItemType type) => SwitchWeapon(type);
        public EquipmentItem[] GetAvailableWeapons => _availableWeap.Values.ToArray();


        #region item object operations
        public override void LoadItem(EquipmentItem item, out string s)
        {
            base.LoadItem(item, out s ); // sets owner
            if (!(item.ItemType == EquipItemType.MeleeWeap || item.ItemType == EquipItemType.RangedWeap)) return;
            else
            {
                _availableWeap[item.ItemType] = item;
                var i = _availableWeap[item.ItemType];

                var config = DataManager.Instance.GetConfigByID<BaseWeaponConfig>(i.ID);
                if (config == null)
                {
                    IsReady = false;
                    throw new Exception($"Failed to config weapon {item.ID} on {Owner}");
                }
                else
                {
                    _cachedConfigs[i] = DataManager.Instance.GetConfigByID<BaseWeaponConfig>(item.ID);
                    var w = i.GetInstantiatedPrefab as BaseWeapon;
                    w.Owner = Owner;    
                    w.SetUpWeapon(_cachedConfigs[i]);


                    switch (item.ItemType)
                    {
                        case EquipItemType.MeleeWeap:
                            w.transform.parent = Empties.MeleeWeaponEmpty;
                            w.transform.SetPositionAndRotation(Empties.MeleeWeaponEmpty.position, Empties.MeleeWeaponEmpty.rotation);
                            Sheathe(item.ItemType);
                            break;
                        case EquipItemType.RangedWeap:
                            w.transform.parent = Empties.RangedWeaponEmpty;
                            w.transform.SetPositionAndRotation(Empties.RangedWeaponEmpty.position, Empties.RangedWeaponEmpty.rotation);
                            Sheathe(item.ItemType);
                            break;
                        default:
                            break;
                    }
                    //i.IsInstantiated = false; 
                    // keep visible for sheathing
                } 
            }
        }

        protected bool Equip(EquipItemType type)
        {
            if (!_availableWeap.ContainsKey(type) || (_availableWeap[type] == null)) return false;
            else
            {
                var weap = _availableWeap[type];
                weap.IsInstantiated = true; // just in case
                var w = weap.GetInstantiatedPrefab;
                
                switch (type)
                {
                    case EquipItemType.MeleeWeap:
                        w.transform.parent = Empties.MeleeWeaponEmpty;
                        w.transform.SetPositionAndRotation(Empties.MeleeWeaponEmpty.position, Empties.MeleeWeaponEmpty.rotation);
                        break;
                    case EquipItemType.RangedWeap:
                        w.transform.parent = Empties.RangedWeaponEmpty;
                        w.transform.SetPositionAndRotation(Empties.RangedWeaponEmpty.position, Empties.RangedWeaponEmpty.rotation);
                        break;
                    default:
                        return false;             
                }

                return true;
            }
        }

        protected virtual void SwitchWeapon(EquipItemType type)
        {
            SwitchAnimationLayersEvent?.Invoke(type);

            foreach (var w in _availableWeap.Keys)
            {
                if (w == type)
                {
                    Equip(type);
                }
                else
                {
                    Sheathe(type);
                }
            }
            
        }


        protected void Sheathe(EquipItemType type)
        {
            var item = _availableWeap[type].GetInstantiatedPrefab;
            item.transform.SetPositionAndRotation(Empties.SheathedWeaponEmpty.position, Empties.SheathedWeaponEmpty.rotation);
            item.transform.parent = Empties.SheathedWeaponEmpty;
        }


        #endregion

        #region ctrl functins

        public virtual bool OnWeaponUseSuccessCheck(EquipItemType type, out string result)
        {
            if (!_availableWeap.ContainsKey(type))
            {
                result = ($"{Owner.GetFullName} used {type} but has no weapon of this type available;");
                return false;
            }
            else
            {
                return (_availableWeap[type].GetInstantiatedPrefab as BaseWeapon).UseWeapon(out result);
            }
        }
        public void ToggleTriggersOnMelee(bool isEnable)
        {
            // todo might get nullref here
                (_availableWeap[EquipItemType.MeleeWeap].GetInstantiatedPrefab as MeleeWeapon).ToggleColliders(isEnable);
        }

        #endregion

        #region managed 
        public override void SetupStatsComponent()
        {
            base.SetupStatsComponent();
        }


        public override void UpdateInDelta(float deltaTime)
        {
            foreach (var w in _availableWeap.Values)
            {
                w.UpdateInDelta(deltaTime);
            }
        }

        public override void StopStatsComponent()
        {
            base.StopStatsComponent();
        }

        #endregion


    }

}