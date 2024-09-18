using Arcatech.Actions;
using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Items
{
    [CreateAssetMenu (fileName = "Melee Weapon Use Strategy",menuName = "Items/Use strategy/Melee Weapon") ]
    public class SerializedMeleeWeaponUseStrategy : SerializedWeaponUseStrategy
    {

       // [SerializeField] SerializedActionResult[] OnStartSwing;
        [SerializeField] SerializedActionResult[] OnColliderHit;
        // [SerializeField] SerializedActionResult[] OnEndSwing;

        private void OnValidate()
        {
            Assert.IsNotNull(OnColliderHit);
            Assert.IsTrue(OnColliderHit.Length > 0);
        }
        public override WeaponStrategy ProduceStrategy(EquippedUnit unit, WeaponSO cfg, BaseWeaponComponent comp)
        {
            return new MeleeWeaponStrategy(OnColliderHit,Action, unit, cfg, TotalCharges,ChargeRestoreTime,comp);
        }
    }

    public class MeleeWeaponStrategy : WeaponStrategy
    {
        public MeleeWeaponStrategy(SerializedActionResult[] onHit, SerializedUnitAction act, EquippedUnit unit, WeaponSO cfg, int charges, float reload, BaseWeaponComponent comp) : base(act, unit, cfg, charges, reload, 0.05f, comp)
        {
            Trigger = (comp as MeleeWeaponComponent).Trigger;
            Trigger.SomethingHitEvent += HandleColliderHitEvent;
            Trigger.ToggleCollider(false);
            _onHit = onHit;
        }

        SerializedActionResult[] _onHit;

        protected WeaponTriggerComponent Trigger;

        public void SwitchCollider(bool state) => Trigger.ToggleCollider(state);

        public override bool TryUseUsable(out BaseUnitAction action)
        {
            bool ok = CheckTimersAndCharges();
            action = null;
            if (!ok) return false;

            // case first attack
            if (_currentAction == null || _currentAction.IsComplete)
            {
                action = Action.ProduceAction(Owner);
                Trigger.ToggleCollider(true);
                _currentAction = action;
                _currentAction.OnComplete += DisableColliderOnCompletedSwing;
                ChargesLogicOnUse();
               // Debug.Log($"Doing first attack");
                return true;
            }
            //case chain
            if (_currentAction != null && !_currentAction.IsComplete)
            {
                if (_currentAction.CanAdvance(out var n))
                {
                    DisableColliderOnCompletedSwing();
                    action = n;
                    _currentAction = n;
                    Trigger.ToggleCollider(true);
                    ChargesLogicOnUse();
                  // Debug.Log($"Doing chain attack");
                    return true;
                }
                else return false;
            }
            Debug.Log($"Can't attack");
            return false;
        }

        protected void DisableColliderOnCompletedSwing()
        {
            _currentAction.OnComplete -= DisableColliderOnCompletedSwing;
            SwitchCollider(false);
        }

        private void HandleColliderHitEvent(Collider target)
        {
            if (target == Owner) return;
            else
            {
                if (target.TryGetComponent<BaseEntity>(out var e))
                {
                    foreach (var res in _onHit)
                    {
                        res.GetActionResult().ProduceResult(Owner, e, WeaponComponent.Spawner);
                    }
                }
            }
        }

    }


}