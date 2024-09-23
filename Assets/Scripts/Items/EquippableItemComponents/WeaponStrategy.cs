using Arcatech.Actions;
using Arcatech.AI;
using Arcatech.UI;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Items
{

    public class WeaponStrategy : IWeaponUseStrategy
    {
        public BaseEntity Owner { get; }
        public WeaponSO Config { get; }
        protected BaseWeaponComponent WeaponComponent { get; }


        public WeaponStrategy (SerializedUnitAction act,SerializedActionResult[] onUse, SerializedActionResult[] onFinishUse, EquippedUnit unit, WeaponSO cfg, int charges, float reload, float intcd,BaseWeaponComponent comp)
        {
            Owner = unit;
            Config = cfg;
            ChargeReload = reload;
            InternalDelay = intcd;
            MaxCharges = charges;
            WeaponComponent = comp;

            Action = act;

            OnActionStart = new IActionResult[onUse.Length];
            for (int i = 0; i < onUse.Length; i++)
            {
                OnActionStart[i] = onUse[i].GetActionResult();
            }

            OnActionComplete = new IActionResult[onFinishUse.Length];
            for (var i = 0; i < onFinishUse.Length; i++)
            {
                OnActionComplete[i] = onFinishUse[i].GetActionResult();
            }

            _remainingCharges = MaxCharges;
            _chargesTimers = new Queue<CountDownTimer>(charges);
            _internalCdTimer = new CountDownTimer(InternalDelay);
            _internalCdTimer.Start();
        }



# region actions
        protected SerializedUnitAction Action { get; }
        protected IActionResult[] OnActionStart { get; }
        protected IActionResult[] OnActionComplete { get; }

        protected void PerformOnStart(BaseEntity user, BaseEntity target, Transform place)
        {
            foreach (var action in OnActionStart)
            {
                action.ProduceResult(user, target, place);
            }
        }
        protected void PerformOnComplete(BaseEntity user, BaseEntity target, Transform place)
        {
            foreach (var action in OnActionComplete)
            {
                action.ProduceResult(user, target, place);
            }
        }

        #endregion


        // charges
        #region charges and cds
        protected int MaxCharges { get; }
        protected float ChargeReload { get; }
        protected float InternalDelay { get; }

        Queue<CountDownTimer> _chargesTimers;
        CountDownTimer _internalCdTimer;
        protected int _remainingCharges { get; private set; }
        protected bool CheckTimersAndCharges()
        {
            if (!_internalCdTimer.IsReady) return false;
            else
            {
                if (_remainingCharges > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private void ReplenishCharge()
        {
            _chargesTimers.Peek().OnTimerStopped -= ReplenishCharge;
            _chargesTimers.Dequeue();
            _remainingCharges++;
        }
        protected void ChargesLogicOnUse()
        {
            var t = new CountDownTimer(ChargeReload);
            _internalCdTimer.Start();
            t.Start();
            _chargesTimers.Enqueue(t);
            _remainingCharges--;
            t.OnTimerStopped += ReplenishCharge;
        }


        #endregion
        #region usable

        public virtual bool TryUseUsable(out BaseUnitAction action)
        {
            action = Action.ProduceAction(Owner);
            if (CheckTimersAndCharges())
            {
                ChargesLogicOnUse(); return true;
            }
            else return false;
        }
        
        public virtual void UpdateUsable(float delta)
        {
            foreach (var t in _chargesTimers.ToList()) 
            { 
                t?.Tick(delta); 
            }
            _internalCdTimer?.Tick(delta);
        }
        #endregion
        #region UI
        public Sprite Icon => Config.Description.Picture;
        public float CurrentNumber => MaxCharges - _chargesTimers.Count;
        public float MaxNumber => MaxCharges;
        #endregion
    }


}