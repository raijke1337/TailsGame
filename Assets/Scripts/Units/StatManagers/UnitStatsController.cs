using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Stats
{
    public class UnitStatsController : ManagedControllerBase
    {
        private Dictionary<BaseStatType, StatValueContainer> _stats { get; set;  }
        public event UnityAction<StatChangedEvent> StatsUpdatedEvent = delegate { };

        [SerializeField, Tooltip("How often the component reports on all stats")] float _freqUpdates = 0.3f;
        private CountDownTimer _updatesCooldownTimer;

        public UnitStatsController(SerializedStatModConfig[] startingstats, DummyUnit dummyUnit) : base(dummyUnit)
        {
            _stats = new Dictionary<BaseStatType, StatValueContainer>();
            var vals = Enum.GetValues(typeof(BaseStatType));
            foreach (var typ in vals)
            {
                _stats[(BaseStatType)typ] = new StatValueContainer();
            }
            AddMods(startingstats);

        }

        public UnitStatsController AddMods (SerializedStatModConfig[] mods)
        {
            foreach (var cfg in mods)
            {
                _stats[cfg.ChangedStat].ApplyStatsMod(new StatsMod(cfg));
            }
            return this;
        }

        public UnitStatsController AddEffect (StatsEffect eff)
        {
            if (_stats.TryGetValue(eff.StatType,out var c))
            {
                c.ApplyStatsEffect(eff);
            }
            RaiseEvent(eff.StatType);
            return this;
        }

        public bool TryApplyCost (StatsEffect cost)
        {
            var cont = _stats[cost.StatType];
            bool OK = cont.GetCurrent >= cost.InitialValue;
            if (OK)
            {
                cont.ApplyStatsEffect(cost);
            }
            return OK;
        }

        #region setup

        #region managed

        public override void StartController()
        {
            foreach (var stat in _stats.Values)
            {
                stat.Initialize(stat.GetMax);
                stat.CachedValue = stat.GetCurrent;
            }
            _updatesCooldownTimer = new CountDownTimer(_freqUpdates);
            _updatesCooldownTimer.OnTimerStopped += () =>
            {
                Owner.StartCoroutine(ReportUpdatedStats());
                _updatesCooldownTimer.Start();
            };

            Owner.StartCoroutine(ReportUpdatedStats());
        }

        

        public override void ControllerUpdate(float delta)
        {
            foreach (var stat in _stats)
            {
                stat.Value.UpdateInDelta(delta);
            }
        }

        public override void StopController()
        {
        }

        public override void FixedControllerUpdate(float fixedDelta)
        {
            
        }
        #endregion
        #endregion

        private IEnumerator ReportUpdatedStats()
        {
            yield return null;
            foreach (var stat in _stats.Keys)
            {
                RaiseEvent(stat);   
            }
        }
        private void RaiseEvent(BaseStatType stat)
        {
            StatsUpdatedEvent.Invoke(new StatChangedEvent(stat, _stats[stat]));
            _stats[stat].CachedValue = _stats[stat].GetCurrent;
        }


    }
}