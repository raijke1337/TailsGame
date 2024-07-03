using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class MeleeWeaponUseStrategy : BaseWeaponUseStrategy
    {
        protected WeaponTriggerComponent Trigger { get; }
        StatsEffect[] currentUseEffects; // to prevent double application

        public MeleeWeaponUseStrategy(MeleeWeaponComponent weapon, DummyUnit owner, SerializedStatsEffectConfig[] effects) : base (owner, effects)
        {
            Trigger = weapon.Trigger;
            Trigger.UnitHitEvent += HandleBaseUnitHitEvent;
        }

        private void HandleBaseUnitHitEvent(DummyUnit target)
        {
            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target, Owner,  true, Trigger.transform, currentUseEffects));
        }

        public override void WeaponUsedStateEnter()
        {
            if (currentUseEffects == null)
            {
                currentUseEffects = new StatsEffect[EffectConfigs.Length];
            }
            for (int i = 0; i < currentUseEffects.Length; i++)  
            {
                currentUseEffects[i] = new StatsEffect(EffectConfigs[i]);
            }
            Trigger.ToggleCollider(true);
        }

        public override void WeaponUsedStateExit()
        {
            Trigger.ToggleCollider(false);
        }        
    }


}