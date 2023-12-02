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

        #region item object operations

        public WeaponController (ItemEmpties em, BaseUnit ow) : base(em, ow)
        {

        }

        protected override void FinishItemConfig(EquipmentItem i)
        {
            var cfg = DataManager.Instance.GetConfigByID<BaseWeaponConfig>(i.ID);
            if (cfg == null)
            {
                throw new Exception($"Mising cfg by ID {i.ID} from item {i} : {this}");
            }
            else
            {
                var w = i.GetInstantiatedPrefab as BaseWeapon;
                w.SetUpWeapon(cfg);
                IsReady = true;
            }
        }
        protected override void InstantiateItem(EquipmentItem i)
        {
            switch (i.ItemType)
            {
                case EquipItemType.MeleeWeap:
                    Sheathe(i.ItemType);
                    break;
                case EquipItemType.RangedWeap:
                    Equip(i.ItemType);
                    break;
            }
        }
        public override EquipmentItem RemoveItem(EquipItemType type)
        {
            var e = _equipment[type];
            _equipment.Remove(type);

            IsReady = ((type == EquipItemType.MeleeWeap && _equipment[EquipItemType.RangedWeap] == null) || (type == EquipItemType.RangedWeap && _equipment[EquipItemType.MeleeWeap] == null));


            return e;
        }

        protected bool Equip(EquipItemType type)
        {
            if (!_equipment.ContainsKey(type) || (_equipment[type] == null)) return false;
            else
            {
                var weap = _equipment[type];
                var w = weap.GetInstantiatedPrefab;
                IsReady = true;

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
        public WeaponEvents<EquipItemType> SwitchAnimationLayersEvent; // also used for layers switch in playerunit

        public void SwitchModels(EquipItemType type) => SwitchWeapon(type);
        protected virtual void SwitchWeapon(EquipItemType type)
        {
            SwitchAnimationLayersEvent?.Invoke(type);


            if (type == EquipItemType.MeleeWeap)
            {
                Equip(type);
                Sheathe(EquipItemType.RangedWeap);
            }
            if (type == EquipItemType.RangedWeap)
            {
                Equip(type);
                Sheathe(EquipItemType.MeleeWeap);
            }            
        }


        protected void Sheathe(EquipItemType type)
        {
            var item = _equipment[type].GetInstantiatedPrefab;
            item.transform.SetPositionAndRotation(Empties.SheathedWeaponEmpty.position, Empties.SheathedWeaponEmpty.rotation);
            item.transform.parent = Empties.SheathedWeaponEmpty;
        }


        #endregion

        #region ctrl functins

        public virtual bool OnWeaponUseSuccessCheck(EquipItemType type, out string result)
        {
            if (!_equipment.ContainsKey(type))
            {
                result = ($"{Owner.GetFullName} used {type} but has no weapon of this type available;");
                return false;
            }
            else
            {
                return (_equipment[type].GetInstantiatedPrefab as BaseWeapon).UseWeapon(out result);
            }
        }
        public void ToggleTriggersOnMelee(bool isEnable)
        {
            // todo might get nullref here
                (_equipment[EquipItemType.MeleeWeap].GetInstantiatedPrefab as MeleeWeapon).ToggleColliders(isEnable);
        }

        #endregion

        #region managed 

        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);      
            
            if (debugEnabled && currentTimer > debugTime)
            {
                Debug.Log($"Breakpoint");
            }

            if (_equipment.TryGetValue(EquipItemType.MeleeWeap, out var s))
            {
                s.GetInstantiatedPrefab.UpdateInDelta(deltaTime);
            }
            if (_equipment.TryGetValue(EquipItemType.RangedWeap, out var r))
            {
                r.GetInstantiatedPrefab.UpdateInDelta(deltaTime);
            }
        }

        public override void StopStatsComponent()
        {
            base.StopStatsComponent();
        }

        #endregion


    }

}