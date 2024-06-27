using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    public class WeaponItem : EquipmentItem, IWeapon
    {

        protected List<SerializedStatsEffectConfig>  _storedTriggerSettings;
        private SerializedStatsEffectConfig _cost;
        protected BaseWeaponComponent _weaponGameobject;

        public StatsEffect GetCost { get => new(_cost); }


        public IDrawItemStrategy DrawStrategy { get; protected set; }
        public UnitActionType UseActionType{ get; protected set; }

        public DummyUnit Owner { get; protected set; }

        protected IWeaponUseStrategy _strat;

        public WeaponItem(Equip cfg) : base(cfg)
        {
        }

        public void UseItem()
        {
            _strat.WeaponUsedStateEnter();
        }

        public IUsableItem AssignStrategy()
        {
            switch (ItemType)
            {
                case EquipmentType.MeleeWeap:
                    _strat = new MeleeWeaponUseStrategy(_weaponGameobject as MeleeWeaponComponent, _storedTriggerSettings.ToArray(), Owner);
                    break;
                case EquipmentType.RangedWeap:

                    Debug.Log($"No ranged weapon use strat for {this}");
                    break;
            }
           return this;
        }

        public INeedsOwner SetOwner(DummyUnit owner)
        {
            var c = _config as Weapon;

            Owner = owner;
            _weaponGameobject = _gameItem as BaseWeaponComponent;
            _cost = c.Cost;
            _storedTriggerSettings = new List<SerializedStatsEffectConfig>(c.UseEffects);

            switch (c.ItemType)
            {
                case EquipmentType.MeleeWeap:
                    UseActionType = UnitActionType.Melee;
                    break;
                case EquipmentType.RangedWeap:
                    UseActionType = UnitActionType.Ranged;
                    break;
            }
            DrawStrategy = c.DrawStrategy;

            return this;
        }
    }
}