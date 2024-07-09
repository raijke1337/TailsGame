using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using Arcatech.UI;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;

namespace Arcatech.Skills
{

    [Serializable]
    public class SkillObjectForControls : ISkill
    {
        public DummyUnit Owner { get ; set; }

        private SerializedSkillConfiguration config;

        public UnitActionType UseActionType => config.UnitActionType;
        public StatsEffect GetCost => new(config.CostTrigger);

        //private BaseSkillUsageStrategy _strat;

        int _charges;
        float _chargeCd;
        float _currCd = 0;

        Queue<CountDownTimer> _chargesTimers;

        public SkillObjectForControls(SerializedSkillConfiguration settings, DummyUnit owner)
        { 
            config = settings;
            Owner = owner;
          //  _strat = new ProjectileSkillStrat(owner.GetSkillTransform(config.UnitActionType),owner, this);


            _charges = config.Charges;
            _chargeCd = config.ChargeRestore;
            _chargesTimers = new Queue<CountDownTimer>();
        }

        public bool TryUseItem()
        {
            if (_chargesTimers.Count >= _charges)
            {
                EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));
                return false;
            }
            if (_currCd > 0)
            {
                EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));
                return false;
            }
            else
            {
                var timer = new CountDownTimer(_chargeCd);

                _chargesTimers.Enqueue(timer);
                timer.Start();
                timer.OnTimerStopped += RemoveTimer;
                _currCd = 0.1f; // placeholder 
                EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));


                //  _strat.SkillUseStateEnter(); // replaced with line below
                config.ProduceProjectile(Owner, Owner.GetSkillTransform(UseActionType), config.Triggers);

                return true;
            }
        }

        private void RemoveTimer()
        {
            _chargesTimers.Dequeue();
            EventBus<IUsableUpdatedEvent>.Raise(new IUsableUpdatedEvent(this, Owner));

        }

        public void DoUpdate(float delta)
        {
            foreach (var timer in _chargesTimers.ToArray())
            {
                timer.Tick(delta);
            }
            _currCd -= delta;
        }

        #region UI


        public Sprite Icon => config.Description.Picture;

        public float CurrentNumber => _charges - _chargesTimers.Count;

        public float MaxNumber => _charges;

        #endregion
    }

}
