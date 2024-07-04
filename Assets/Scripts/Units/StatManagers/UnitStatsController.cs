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
        private Dictionary<BaseStatType, StatValueContainer> stats { get; set;  }
        public event UnityAction<StatChangedEvent> StatsUpdatedEvent = delegate { };

        [SerializeField, Tooltip("How often the component reports on all stats")] float _freqUpdates = 0.3f;
        private CountDownTimer _updatesCooldownTimer;

        public UnitStatsController(SerializedStatModConfig[] startingstats, DummyUnit dummyUnit) : base(dummyUnit)
        {
            stats = new Dictionary<BaseStatType, StatValueContainer>();
            var vals = Enum.GetValues(typeof(BaseStatType));
            foreach (var typ in vals)
            {
                stats[(BaseStatType)typ] = new StatValueContainer();
            }
            AddMods(startingstats);

        }

        public UnitStatsController AddMods (SerializedStatModConfig[] mods)
        {
            foreach (var cfg in mods)
            {
                stats[cfg.ChangedStat].ApplyStatsMod(new StatsMod(cfg));
            }
            return this;
        }

        public bool TryAddEffect (StatsEffect eff)
        {
            if (stats.TryGetValue(eff.StatType,out var c))
            {
                c.ApplyStatsEffect(eff);
                RaiseEvent(eff.StatType);
                return true;
            }
            return false;
        }

        public bool TryApplyCost (StatsEffect cost)
        {
            var cont = stats[cost.StatType];
            bool OK = cont.GetCurrent >= Mathf.Abs(cost.InitialValue);
            if (OK)
            {
                cont.ApplyStatsEffect(cost);
                RaiseEvent (cost.StatType);
            }
            return OK;
        }

        #region managed

        public override void StartController()
        {
            foreach (var stat in stats.Values)
            {
                stat.Initialize(stat.GetMax);
                stat.CachedValue = stat.GetCurrent;
            }
            _updatesCooldownTimer = new CountDownTimer(_freqUpdates);
            _updatesCooldownTimer.Start();
            _updatesCooldownTimer.OnTimerStopped += RefreshTimer;

            Owner.StartCoroutine(ReportUpdatedStats());
        }

        private void RefreshTimer()
        {
            Owner.StartCoroutine(ReportUpdatedStats());
            _updatesCooldownTimer.Start();
        }

        public override void ControllerUpdate(float delta)
        {
            foreach (var stat in stats)
            {
                stat.Value.UpdateInDelta(delta);
            }
            _updatesCooldownTimer.Tick(delta);
        }

        public override void StopController()
        {
        }

        public override void FixedControllerUpdate(float fixedDelta)
        {
            
        }
        #endregion
        private IEnumerator ReportUpdatedStats()
        {
            yield return null;
            foreach (var stat in stats.Keys)
            {
                RaiseEvent(stat);   
            }
        }
        private void RaiseEvent(BaseStatType stat)
        {
            StatsUpdatedEvent.Invoke(new StatChangedEvent(stat, stats[stat]));
            stats[stat].CachedValue = stats[stat].GetCurrent;
        }


    }
}