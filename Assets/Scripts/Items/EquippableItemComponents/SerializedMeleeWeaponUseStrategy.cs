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
            bool ok = CheckTimersAndCharges();
            action = null;
            if (!ok) return false;

            // case first attack
            if (_currentAction == null || _currentAction.IsComplete)
            {
                action = Action.ProduceAction(Owner);
                ResetEffects();
                Trigger.ToggleCollider(true);
                _currentAction = action;
                ChargesLogicOnUse();
                Debug.Log($"Doing first attack");
                return true;
            }
            //case chain
            if (_currentAction != null && !_currentAction.IsComplete)
            {
                if (_currentAction.CanAdvance(out var n))
                {
                    action = n;
                    _currentAction = n;
                    ResetEffects();
                    Trigger.ToggleCollider(true);
                    ChargesLogicOnUse();
                    Debug.Log($"Doing chain attack");
                    return true;
                }
                else return false;
            }
            Debug.Log($"Can't attack");
            return false;
        }
        protected void OnActionsComplete()
        {
            Trigger.ToggleCollider(false);
        }
        private void HandleBaseUnitHitEvent(DummyUnit target)
        {
            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target, Owner, true, Trigger.transform, currentUseEffects));
        }
    }


}