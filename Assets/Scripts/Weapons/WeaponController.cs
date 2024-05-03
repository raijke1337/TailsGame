using Arcatech.Effects;
using Arcatech.Items;
using System;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class WeaponController : BaseControllerConditional
    {

        #region item object operations

        public WeaponController(ItemEmpties em, BaseUnit ow) : base(em, ow)
        {

        }

        protected override void FinishItemConfig(EquipmentItem i)
        {
            if (i is RangedWeaponItem rr)
            {
                rr.PlacedProjectileEvent += SpawnProjectileCallBack;
            }
            IsReady = true;
        }


        protected override void InstantiateItem(EquipmentItem i)
        {
            if (Owner is PlayerUnit p)
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
            if (Owner is NPCUnit n)
            {
                switch (i.ItemType)
                {
                    case EquipItemType.MeleeWeap:
                        Equip(i.ItemType);
                        break;
                    case EquipItemType.RangedWeap:
                        Equip(i.ItemType);
                        break;
                }
            }

        }
        public override bool TryRemoveItem(EquipItemType type, out EquipmentItem removed)
        {

            var re = base.TryRemoveItem(type,out removed);
            if (re)
            {
                IsReady = ((type == EquipItemType.MeleeWeap && _equipment[EquipItemType.RangedWeap] == null) ||
                    (type == EquipItemType.RangedWeap && _equipment[EquipItemType.MeleeWeap] == null));
            }

            return re;
        }

        protected bool Equip(EquipItemType type)
        {
            if (!_equipment.ContainsKey(type) || (_equipment[type] == null)) return false;
            else
            {
                var weap = _equipment[type];
                IsReady = true;

                switch (type)
                {
                    case EquipItemType.MeleeWeap:
                        weap.SetItemEmpty(Empties.ItemPositions[EquipItemType.MeleeWeap]);
                        break;
                    case EquipItemType.RangedWeap:
                        weap.SetItemEmpty(Empties.ItemPositions[EquipItemType.RangedWeap]);
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
            if (_equipment.TryGetValue(type, out var equip))
            {
                equip.SetItemEmpty(Empties.ItemPositions[EquipItemType.Other]);
            }
        }


        #endregion

        #region ctrl functins

        public virtual bool OnWeaponUseSuccessCheck(EquipItemType type)
        {
            if (!_equipment.ContainsKey(type))
            {
                return false;
            }
            else
            {
                bool ok;
                switch (type)
                {
                    case EquipItemType.RangedWeap:
                        var ranged = _equipment[type] as RangedWeaponItem;
                        ok = ranged.TryUseItem();
                        break;
                    default:
                       var weap = (_equipment[type] as WeaponItem);
                        ok = weap.TryUseItem();

                        break;
                }
                if (ok)
                {
                    EffectEventCallback(new EffectRequestPackage(_equipment[type].Effects.GetRandomSound(EffectMoment.OnStart), _equipment[type].Effects.GetRandomEffect(EffectMoment.OnStart),
                        null,
                        _equipment[type].GetInstantiatedPrefab().transform));
                    SwitchWeapon(type);
                }
                return ok;
            }
        }
        public void ToggleTriggersOnMelee(bool isEnable)
        {
            // todo might get nullref here
            (_equipment[EquipItemType.MeleeWeap].GetInstantiatedPrefab() as MeleeWeaponComponent).ToggleColliders(isEnable);
        }

        #endregion

        #region managed 



        public override void StopStatsComponent()
        {
            base.StopStatsComponent();
        }

        #endregion
        public override string GetUIText { get => ($""); } // unused because in UI skill cd is displayed

    }

}