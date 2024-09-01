using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
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
        public IWeaponUseStrategy UseStrategy { get; protected set; }



        public Weapon(WeaponSO cfg, DummyUnit ow) : base(cfg, ow)
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
            UseStrategy = cfg.WeaponUseStrategy.ProduceStrategy(Owner, cfg,_weaponGameobject);
        }

        public bool TryUseItem(out BaseUnitAction act)
        {
            bool ok = UseStrategy.TryUseUsable(out act);
            return ok;
        }

        public void DoUpdate(float delta)
        {
            UseStrategy.UpdateUsable(delta);
            EventBus<UpdateIconEvent>.Raise(new UpdateIconEvent(this, Owner));
        }

        #region UI

        public Sprite Icon => Config.Description.Picture;

        public float CurrentNumber => UseStrategy.CurrentNumber;

        public float MaxNumber => UseStrategy.MaxNumber;


        #endregion
    }
}