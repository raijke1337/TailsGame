using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.UI;
using Arcatech.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        int _charges;
        float _chargeCd;
        float _useCd;
        float _currCd = 0;

        Queue<CountDownTimer> _chargesTimers;



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

            _chargeCd = cfg.ChargeRestoreTime;
            _charges = cfg.Charges;
            _useCd = cfg.InternalCooldown;


            switch (Type)
            {
                case EquipmentType.MeleeWeap:
                    _strat = new MeleeWeaponUseStrategy(_weaponGameobject as MeleeWeaponComponent, Owner, _storedTriggerSettings.ToArray());
                    break;
                case EquipmentType.RangedWeap:
                    _strat = new RangedWeaponUseStrategy(_weaponGameobject as RangedWeaponComponent, Owner, _storedTriggerSettings.ToArray());
                    break;
            }


            _chargesTimers = new Queue<CountDownTimer>(_charges);

        }


        public bool TryUseItem()
        {
            if (_chargesTimers.Count >= _charges)
            {
              //  Debug.Log("Fail to use weapon - no charges");
                EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));
                return false;
            }
            if (_currCd > 0)
            {
               // Debug.Log("Fail to use weapon - internal cd");
                EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));
                return false;
            }
            else
            {
                var timer = new CountDownTimer(_chargeCd);
                
                _chargesTimers.Enqueue(timer);
                timer.Start();
                timer.OnTimerStopped += RemoveTimer;

                _strat.WeaponUsedStateEnter();
                _currCd = _useCd;
                EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));

                return true;
            }
        }

        private void RemoveTimer()
        {
            _chargesTimers.Dequeue();
            EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));

        }

        public void DoUpdate(float delta)
        {
            foreach (var timer in _chargesTimers.ToArray())
            {
                timer.Tick(delta);
            }
            _currCd -= delta;
        }


        #region UI

        public Sprite Icon => Config.Description.Picture;

        public float CurrentNumber => _charges - _chargesTimers.Count;

        public float MaxNumber => _charges;

        #endregion
    }
}