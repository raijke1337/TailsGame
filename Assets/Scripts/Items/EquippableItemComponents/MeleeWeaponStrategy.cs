using Arcatech.Actions;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class MeleeWeaponStrategy : WeaponStrategy
    {
        public MeleeWeaponStrategy(SerializedActionResult[] onHit, SerializedUnitAction act, SerializedActionResult[] onSt, SerializedActionResult[] onE, EquippedUnit unit, WeaponSO cfg, int charges, float reload, BaseWeaponComponent comp) : base(act, onSt, onE,unit, cfg, charges, reload, 0.05f, comp)
        {
            Trigger = (comp as MeleeWeaponComponent).Trigger;
            Trigger.SomethingHitEvent += HandleColliderHitEvent;
            Trigger.ToggleCollider(false);

            OnColliderHit = new IActionResult[onHit.Length];

            for (int i = 0; i < onHit.Length; i++)
            {
                OnColliderHit[i] = onHit[i].GetActionResult();
            }
            _actionBandaidTimer = new CountDownTimer(reload);

            _actionBandaidTimer.OnTimerStopped += BandaidTimer;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += Cleanup;

        }



        private void Cleanup(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            Trigger.SomethingHitEvent -= HandleColliderHitEvent;
            _actionBandaidTimer.OnTimerStopped -= BandaidTimer;
           // Debug.Log($"weapon {WeaponComponent} unsub");
        }

        void BandaidTimer ()
        {            
            _currentAction = null;
            PerformOnComplete(Owner, null, WeaponComponent.Spawner);
        }

        protected IActionResult[] OnColliderHit { get; }

        protected WeaponTriggerComponent Trigger;
        BaseUnitAction _currentAction;
        CountDownTimer _actionBandaidTimer;

        public void SwitchCollider(bool state) => Trigger.ToggleCollider(state);

        public override bool TryUseUsable(out BaseUnitAction action)
        {
            // TODO needs debug
            // add checks to prevent additional triggering

            bool ok = CheckTimersAndCharges();
            action = null;
            if (!ok) return false;

            // case first attack
            if (_currentAction == null || _currentAction.IsComplete)
            {
                PerformOnComplete(Owner, null, WeaponComponent.Spawner);

                action = Action.ProduceAction(Owner);
                _currentAction = action;
                _actionBandaidTimer.Reset();
                _actionBandaidTimer.Start();

                ChargesLogicOnUse();
                PerformOnStart(Owner, null, WeaponComponent.Spawner);

                return true;
            }
            //case chain
            if (_currentAction != null && !_currentAction.IsComplete)
            {
                if (_currentAction.CanAdvance(out var n))
                {
                    PerformOnComplete(Owner, null, WeaponComponent.Spawner);

                    action = n;
                    _currentAction = n;
                    ChargesLogicOnUse();
                    _actionBandaidTimer.Reset();
                    _actionBandaidTimer.Start();

                    PerformOnStart(Owner, null, WeaponComponent.Spawner);
                    // Debug.Log($"Doing chain attack");
                    return true;
                }
                else return false;
            }
            Debug.Log($"Can't attack");
            return false;
        }

        protected  void PerformOnHit(BaseEntity user, BaseEntity target, Transform place)
        {
            foreach (var res in OnColliderHit)
            {
                res.ProduceResult(user, target, place);
            }
        }

        private void HandleColliderHitEvent(Collider target)
        {
            if (target == Owner) return;
            else
            {
                if (target.TryGetComponent<BaseEntity>(out var e))
                {
                    PerformOnHit(Owner, e, WeaponComponent.Spawner);
                }
            }
        }

        public override void UpdateUsable(float delta)
        {
            base.UpdateUsable(delta);
            _actionBandaidTimer?.Tick(delta);
        }


    }


}