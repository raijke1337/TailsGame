using Arcatech.Actions;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using Arcatech.UI;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Skills
{
    public  class SkillUsageStrategy : IUsablesStrategy    , IIconContent
    {
        public BaseUnit Owner {get;protected set;}
        SerializedUnitAction Action { get; }
        IActionResult[] SkillUsageResults;

        protected BaseEquippableItemComponent Spawner;
        ExtendedText _desc;



        int MaxCharges { get; }
        float ChargeReload { get; }
        float InternalDelay { get; }

        Queue<CountDownTimer> _chargesTimers;
        CountDownTimer _internalCdTimer;
        int _remainingCharges;



        public SkillUsageStrategy(SerializedActionResult[] results, BaseEquippableItemComponent spawner, SerializedUnitAction useaction, BaseUnit unit, SerializedSkill cfg, int charges, float reload)
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
            Spawner = spawner;
        }

        public bool TryUseUsable(out BaseUnitAction action)
        {
            action = Action.ProduceAction(Owner);
            if (!_internalCdTimer.IsReady || !action.IsComplete) return false;
            else
            {
                if (_remainingCharges > 0)
                {
                    var t = new CountDownTimer(ChargeReload);
                    _internalCdTimer.Start();
                    t.Start();
                    _chargesTimers.Enqueue(t);
                    _remainingCharges--;
                    t.OnTimerStopped += () => _chargesTimers.Dequeue();
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

        #region UI
        public Sprite Icon => _desc.Picture;
        public float CurrentNumber => MaxCharges - _chargesTimers.Count;
        public float MaxNumber => MaxCharges;

        #endregion

    }
}