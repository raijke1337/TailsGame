using Arcatech.UI;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Items
{
    public class WeaponStrategy : IWeaponUseStrategy
    {
        public BaseUnit Owner { get; }
        public WeaponSO Config { get; }
        protected BaseWeaponComponent WeaponComponent { get; }
        protected SerializedUnitAction Action { get; }

        int MaxCharges { get; }
        float ChargeReload { get; }
        float InternalDelay { get; }

        Queue<CountDownTimer> _chargesTimers;
        CountDownTimer _internalCdTimer;
        int _remainingCharges;

        

        public WeaponStrategy (SerializedUnitAction act,DummyUnit unit, WeaponSO cfg, int charges, float reload, float intcd,BaseWeaponComponent comp)
        {
            Owner = unit;
            Config = cfg;
            ChargeReload = reload;
            InternalDelay = intcd;
            MaxCharges = charges;
            WeaponComponent = comp;
            Action = act;

            _remainingCharges = MaxCharges;
            _chargesTimers = new Queue<CountDownTimer>(charges);
            _internalCdTimer = new CountDownTimer(InternalDelay);
            _internalCdTimer.Start();
        }

        public virtual bool TryUseItem(out BaseUnitAction action)
        {
            action = Action.ProduceAction(Owner);
            if (!_internalCdTimer.IsReady || !action.IsDone ) return false;
            else
            {
                if (_remainingCharges > 0)
                {
                    var t = new CountDownTimer(ChargeReload);
                    _internalCdTimer.Start();
                    t.Start();
                    _chargesTimers.Enqueue(t);
                    _remainingCharges--;
                    t.OnTimerStopped += ReplenishCharge;
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

        protected virtual void OnActionsComplete()
        {
            Debug.Log($"finished using {this}");
        }


        public virtual void Update(float delta)
        {
            foreach (var t in _chargesTimers.ToList()) 
            { 
                t?.Tick(delta); 
            }
            _internalCdTimer?.Tick(delta);
        }

        #region UI
        public Sprite Icon => Config.Description.Picture;
        public float CurrentNumber => MaxCharges - _chargesTimers.Count;
        public float MaxNumber => MaxCharges;
        #endregion
    }


}