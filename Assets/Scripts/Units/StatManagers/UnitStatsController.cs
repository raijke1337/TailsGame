using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Stats
{
    [Serializable]
    public class UnitStatsController : ManagedControllerBase
    {
        private Dictionary<BaseStatType, StatValueContainer> _stats;

        public UnitStatsController(SerializedStatModConfig[] startingstats, BaseEntity dummyUnit) : base(dummyUnit)
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
                _stats[cfg.GetStatType].ApplyStatsMod(cfg);
            }
            return this;
        }

        public bool CanApplyEffect (StatsEffect eff,out float current)
        {
            if (_stats.TryGetValue(eff.StatType,out var c))
            {
                c.ApplyStatsEffect(eff);
                current = c.GetCurrent;
                return true;
            }
            else
            {
                current = 0;
                return false;
            }
        }
        public bool CanApplyCost (StatsEffect cost)
        {
            bool OK;
            if (cost == null)
            {
                OK = true;
            }
            else
            {
                var cont = _stats[cost.StatType];
                OK = cont.GetCurrent >= Mathf.Abs(cost.InitialValue);
                if (OK)
                {
                    cont.ApplyStatsEffect(cost);
                } 
            }
            return OK;
        }

        public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetStatValues => _stats;


        #region managed

        public override void StartController()
        {
            foreach (var stat in _stats.Values)
            {
                stat.Initialize(stat.GetMax);
            }
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




    }
}