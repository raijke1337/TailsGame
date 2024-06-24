using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    public class WeaponItem : EquipmentItem
    {

        protected List<SerializedStatsEffectConfig>  _storedTriggerSettings;
        protected List<StatsEffect> _currentUseEffects;

        protected readonly float UseCooldown;
        protected float _currentCD;
        public WeaponItem(Weapon cfg, BaseUnit ow) : base(cfg, ow)
        {
            _storedTriggerSettings = new List<SerializedStatsEffectConfig>(cfg.WeaponHitTriggers);
            UseCooldown = cfg.WeaponCooldown;
            _currentCD = 0;
        }

        protected override void GenerateObject()
        {
            base.GenerateObject();
            if (_instantiated is BaseWeaponComponent weap)
            {
                weap.PrefabCollisionEvent += OnWeapTriggerEvent;
            }
            _currentUseEffects = new List<StatsEffect>();
            foreach (var cfg in _storedTriggerSettings)
            {
                _currentUseEffects.Add(new StatsEffect(cfg));
            }
        }
        public virtual bool TryUseItem()
        { // ranged has ist own logic
            bool ok = _currentCD <= 0;
            if (ok)
            {
                //Debug.LogWarning($"Try use item in {this}, created new effect triggers");
                _currentUseEffects = new List<StatsEffect>();
                foreach (var cfg in _storedTriggerSettings)
                {
                    _currentUseEffects.Add(new StatsEffect(cfg));
                }
                _currentCD = UseCooldown;
            }
            return ok;
        }
        public override void DoUpdates(float d)
        {
            if (_currentCD > 0)
            {
                _currentCD -= d;
            }
        }

        public event TriggerEvent PrefabTriggerHitSomething;

        protected void OnWeapTriggerEvent(BaseUnit target, bool isEnter)
        {            
            if (isEnter && target != Owner)            
            {
                foreach (var ef in _currentUseEffects)
                {
                    PrefabTriggerHitSomething?.Invoke(target, Owner, isEnter, ef);
                }
            }
        }

        

    }
}