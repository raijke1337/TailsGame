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

        protected List<BaseStatTriggerConfig>  WeaponHitEffects;
        protected readonly float UseCooldown;
        protected float _currentCD;
        public WeaponItem(Weapon cfg, BaseUnit ow) : base(cfg, ow)
        {
            WeaponHitEffects = new List<BaseStatTriggerConfig>(cfg.WeaponHitTriggers);
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
        }
        public virtual bool TryUseItem()
        {
            bool ok = _currentCD <= 0;
            if (ok)
            {
                _currentCD = UseCooldown;
                _instantiated.OnItemUse();
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
            if (isEnter)
            {
                foreach (var ef in WeaponHitEffects)
                {
                    PrefabTriggerHitSomething?.Invoke(target, Owner, isEnter, ef);
                }
            }
        }

        

    }
}