using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Triggers;
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
                    case EquipmentType.MeleeWeap:
                        Sheathe(i.ItemType);
                        break;
                    case EquipmentType.RangedWeap:
                        Equip(i.ItemType);
                        break;
                }
            }
            if (Owner is NPCUnit n)
            {
                switch (i.ItemType)
                {
                    case EquipmentType.MeleeWeap:
                        Equip(i.ItemType);
                        break;
                    case EquipmentType.RangedWeap:
                        Equip(i.ItemType);
                        break;
                }
            }

        }
        public override bool TryRemoveItem(EquipmentType type, out EquipmentItem removed)
        {

            var re = base.TryRemoveItem(type,out removed);
            if (re)
            {
                IsReady = ((type == EquipmentType.MeleeWeap && _items[EquipmentType.RangedWeap] == null) ||
                    (type == EquipmentType.RangedWeap && _items[EquipmentType.MeleeWeap] == null));
            }

            return re;
        }

        protected bool Equip(EquipmentType type)
        {
            if (!_items.ContainsKey(type) || (_items[type] == null)) return false;
            else
            {
                var weap = _items[type];
                IsReady = true;

                switch (type)
                {
                    case EquipmentType.MeleeWeap:
                        weap.SetItemEmpty(Empties.ItemPositions[EquipmentType.MeleeWeap]);
                        break;
                    case EquipmentType.RangedWeap:
                        weap.SetItemEmpty(Empties.ItemPositions[EquipmentType.RangedWeap]);
                        break;
                    default:
                        return false;
                }

                return true;
            }
        }
        public WeaponEvents<RuntimeAnimatorController> SwitchAnimationLayersEvent;

        public void SwitchModels(EquipmentType type) => SwitchWeapon(type);

        protected virtual void SwitchWeapon(EquipmentType type)
        {
            SwitchAnimationLayersEvent?.Invoke(_items[type].AnimatorController);
            if (type == EquipmentType.MeleeWeap)
            {
                Equip(type);
                Sheathe(EquipmentType.RangedWeap);
            }
            if (type == EquipmentType.RangedWeap)
            {
                Equip(type);
                Sheathe(EquipmentType.MeleeWeap);
            }
        }


        protected void Sheathe(EquipmentType type)
        {
            if (_items.TryGetValue(type, out var equip))
            {
                equip.SetItemEmpty(Empties.ItemPositions[EquipmentType.Other]);
            }
        }


        #endregion

        #region ctrl functins

        public virtual bool OnWeaponUseSuccessCheck(EquipmentType type)
        {
            if (!_items.ContainsKey(type))
            {
                return false;
            }
            else
            {
                bool ok;
                switch (type)
                {
                    case EquipmentType.RangedWeap:
                        var ranged = _items[type] as RangedWeaponItem;
                        ok = ranged.TryUseItem();
                        break;
                    default:
                       var weap = (_items[type] as WeaponItem);
                        ok = weap.TryUseItem();

                        break;
                }
                if (ok)
                {
                    EffectEventCallback(new EffectRequestPackage(_items[type].Effects.GetRandomSound(EffectMoment.OnStart), _items[type].Effects.GetRandomEffect(EffectMoment.OnStart),
                        null,
                        _items[type].GetInstantiatedPrefab().transform));
                    SwitchWeapon(type);
                }
                return ok;
            }
        }
        public void ToggleTriggersOnMelee(bool isEnable)
        {
            // todo might get nullref here
            (_items[EquipmentType.MeleeWeap].GetInstantiatedPrefab() as MeleeWeaponComponent).ToggleColliders(isEnable);
        }

        #endregion

        #region managed 



        public override void StopComp()
        {

        }

        public override void ApplyEffect(TriggeredEffect effect)
        {

        }

        public override void UpdateInDelta(float deltaTime)
        {
            if (IsReady)
            {
                _items[EquipmentType.MeleeWeap].DoUpdates(deltaTime);
                //_items[EquipItemType.RangedWeap].DoUpdates(deltaTime);
            }
        }

        public override void StartComp()
        {

        }

        #endregion

    }

}