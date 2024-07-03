using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    public class Weapon : Equipment, IWeapon
    {

        protected List<SerializedStatsEffectConfig> _storedTriggerSettings;
        private SerializedStatsEffectConfig _cost;
        protected BaseWeaponComponent _weaponGameobject;

        public StatsEffect GetCost { get => new(_cost); }


        public IDrawItemStrategy DrawStrategy { get; protected set; }
        public UnitActionType UseActionType { get; protected set; }


        protected IWeaponUseStrategy _strat;

        public Weapon(WeaponSO cfg,DummyUnit ow) : base(cfg,ow)
        {
            _weaponGameobject = DisplayItem as BaseWeaponComponent;

            _cost = cfg.Cost;
            _storedTriggerSettings = new List<SerializedStatsEffectConfig>(cfg.UseEffects);

            switch (Type)
            {
                case EquipmentType.MeleeWeap:
                    UseActionType = UnitActionType.Melee;
                    break;
                case EquipmentType.RangedWeap:
                    UseActionType = UnitActionType.Ranged;
                    break;
            }
            DrawStrategy = cfg.DrawStrategy;
        }

        public void UseItem()
        {
            _strat.WeaponUsedStateEnter();
        }

        public IUsableItem AssignStrategy()
        {
            switch (Type)
            {
                case EquipmentType.MeleeWeap:
                    _strat = new MeleeWeaponUseStrategy(_weaponGameobject as MeleeWeaponComponent, Owner,_storedTriggerSettings.ToArray());
                    break;
                case EquipmentType.RangedWeap:
                    _strat = new RangedWeaponUseStrategy(_weaponGameobject as RangedWeaponComponent, Owner,_storedTriggerSettings.ToArray());
                    break;
            }
            return this;
        }
    }
}