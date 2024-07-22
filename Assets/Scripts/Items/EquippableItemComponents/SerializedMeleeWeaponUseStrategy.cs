using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    [CreateAssetMenu (fileName = "Melee Weapon Use Strategy",menuName = "Strategy/Melee Weapon Use") ]
    public class SerializedMeleeWeaponUseStrategy : SerializedWeaponUseStrategy
    {
        public override WeaponStrategy ProduceStrategy(DummyUnit unit, WeaponSO cfg, BaseWeaponComponent comp)
        {
            return new MeleeWeaponStrategy(Action, unit, cfg, TotalCharges,ChargeRestoreTime,InternalCooldown, comp);
        }
    }

    public class MeleeWeaponStrategy : WeaponStrategy
    {

        StatsEffect[] currentUseEffects; // to prevent double application
        void ResetEffects()
        {
            for (int i = 0; i < currentUseEffects.Length; i++)
            {
                currentUseEffects[i] = new StatsEffect(Config.UseEffects[i]);
            }
        }


        protected WeaponTriggerComponent Trigger;

        public MeleeWeaponStrategy(SerializedUnitAction act, DummyUnit unit, WeaponSO cfg, int charges, float reload, float intcd, BaseWeaponComponent comp) : base(act, unit, cfg, charges, reload, intcd, comp)
        {
            Trigger = (comp as MeleeWeaponComponent).Trigger;
            Trigger.UnitHitEvent += HandleBaseUnitHitEvent;
            Trigger.ToggleCollider(false);
            currentUseEffects = new StatsEffect[cfg.UseEffects.Length];
        }
        public override bool TryUseItem(out BaseUnitAction action)
        {
            bool ok =  base.TryUseItem(out action);
            if (ok)
            {
                ResetEffects();
                Trigger.ToggleCollider(true);
            }
            else
            {
                if (action.NextAction != null)
                {
                    ResetEffects();
                    action = action.NextAction;
                }
            }
            return ok;
        }
        protected override void OnActionsComplete()
        {
            Trigger.ToggleCollider(false);
            base.OnActionsComplete();
        }



        private void HandleBaseUnitHitEvent(DummyUnit target)
        {
            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target, Owner, true, Trigger.transform, currentUseEffects));
        }
    }


}