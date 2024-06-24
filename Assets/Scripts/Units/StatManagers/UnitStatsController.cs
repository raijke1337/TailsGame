using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers;
using Arcatech.Units;
using Arcatech.Units.Stats;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Compilation;
using UnityEngine;

namespace Arcatech.Stats
{
    public class UnitStatsController : ManagedControllerBase
    {
        private Dictionary<BaseStatType, StatValueContainer> _stats { get; set;  }

        public event SimpleEventsHandler ZeroHealthEvent; // dead
        public event SimpleEventsHandler StunEvent; // just stun
        public event SimpleEventsHandler KnockDownEvent; // fall, armor damage
        public event SimpleEventsHandler<float> UnitTookDamageEvent; //        

        public UnitStatsController PopulateDictionary()
        {
            _stats = new Dictionary<BaseStatType, StatValueContainer>();
            var vals = Enum.GetValues(typeof(BaseStatType));
            foreach (var typ in vals)
            {
                _stats[(BaseStatType)typ] = new StatValueContainer();
            }
            return this;
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
            return this;
        }
        #region managed

        public override void StartController()
        {
            foreach (var stat in _stats.Values)
            {
                stat.Initialize(stat.GetMax);
            }
            _isReady = true;
            base.StartController();

        }
        public override void UpdateController(float delta)
        {
            if (!_isReady) return;

            foreach (var stat in _stats)
            {
                stat.Value.UpdateInDelta(delta);
            }
        }

        public override void StopController()
        {
            _isReady = false;
        }
        #endregion
    }
}