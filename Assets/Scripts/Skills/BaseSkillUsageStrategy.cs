using Arcatech.Actions;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.UI;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Skills
{
    public  class SkillUsageStrategy : IUsablesStrategy    , IIconContent
    {
        public BaseEntity Owner {get;protected set;}
        SerializedUnitAction Action { get; }
        IActionResult[] SkillUsageResults;

        protected Transform Spawner;
        readonly ExtendedText _desc;



        int MaxCharges { get; }
        float ChargeReload { get; }
        float InternalDelay { get; }

        Queue<CountDownTimer> _chargesTimers;
        CountDownTimer _internalCdTimer;
        int _remainingCharges;



        public SkillUsageStrategy(SerializedActionResult[] results, BaseEquippableItemComponent item, SerializedUnitAction useaction, BaseEntity unit, SerializedSkill cfg, int charges, float reload)
        {
            SkillUsageResults = new ActionResult[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                SkillUsageResults[i] = results[i].GetActionResult();
            }
            Owner = unit;
            _desc = cfg.Description;
            ChargeReload = reload;
            InternalDelay = 0.1f; // placeholder?
            MaxCharges = charges;
            Action = useaction;

            _remainingCharges = MaxCharges;
            _chargesTimers = new Queue<CountDownTimer>(charges);
            _internalCdTimer = new CountDownTimer(InternalDelay);
            _internalCdTimer.Start();
            Spawner = item.Spawner;
        }

        public bool TryUseUsable(out BaseUnitAction action)
        {
            action = Action.ProduceAction(Owner);
            if (!_internalCdTimer.IsReady) return false;
            else
            {
                if (_remainingCharges > 0)
                {
                    var t = new CountDownTimer(ChargeReload);
                    _internalCdTimer.Start();
                    t.Start();
                    _chargesTimers.Enqueue(t);
                    _remainingCharges--;
                    t.OnTimerStopped += OnTimerComplete;
                    DoSkillLogic();
                    return true;
                }
            }
            return false;
        }

        protected virtual void DoSkillLogic()
        {
            foreach (var r in SkillUsageResults)
            {
                r.ProduceResult(Owner, null, Spawner.transform);
            }
        }

        public virtual void UpdateUsable(float delta)
        {
            foreach (var t in _chargesTimers.ToList())
            {
                t?.Tick(delta);
            }
            _internalCdTimer?.Tick(delta);
        }

        void OnTimerComplete()
        {
            _chargesTimers.Dequeue();
            _remainingCharges++;
            Mathf.Clamp(_remainingCharges, 0, MaxCharges); // just in case
        }

        #region UI
        public Sprite Icon => _desc.Picture;
        public float CurrentNumber => _remainingCharges;
        public float MaxNumber => MaxCharges;

        #endregion

    }
}