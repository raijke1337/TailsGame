using Arcatech.EventBus;
using Arcatech.Items;
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
            //var vals = Enum.GetValues(typeof(BaseStatType));
            //foreach (var typ in vals)
            //{
            //    _stats[(BaseStatType)typ] = new StatValueContainer();
            //}
            AddMods(startingstats);
        }
        public UnitStatsController AddMods (SerializedStatModConfig[] mods)
        {
            foreach (var cfg in mods)
            {
                if (!_stats.ContainsKey(cfg.GetStatType)) _stats[cfg.GetStatType] = new StatValueContainer();
                _stats[cfg.GetStatType].ApplyStatsMod(cfg);
            }
            return this;
        }

        public bool CanApplyEffect (StatsEffect eff,out float current, IEquippable withShield = null)
        {
            current = 0;
            StatValueContainer c;
            switch (eff.StatType)
            {
                case BaseStatType.Health:
                    if (withShield != null && eff.InitialValue < 0)
                    {
                        var shield = withShield as Shield;
                        var results = shield.AbsorbStrategy.SplitDamage(eff, _stats[BaseStatType.Energy]);
                        foreach (var result in results)
                        {
                            CanApplyEffect(result, out _, null);
                        }
                        current = _stats[BaseStatType.Health].GetCurrent;
                        shield.AbsorbStrategy.OnApplicationResult.ProduceResult(Owner, Owner, Owner.transform);

                        return true;
                    }
                    else
                    {
                        if (_stats.TryGetValue(eff.StatType, out c))
                        {
                            c.ApplyStatsEffect(eff);
                            if (eff.OnApply!=null)
                            {
                                eff.OnApply.GetActionResult().ProduceResult(Owner, Owner, Owner.transform);
                            }
                            current = c.GetCurrent;
                            return true;
                        }
                    }
                    break;
                default:
                    if (_stats.TryGetValue(eff.StatType, out c))
                    {
                        c.ApplyStatsEffect(eff);
                        current = c.GetCurrent;
                        return true;
                    }
                    break;
            }
            Debug.Log($"Can't apply effect {eff}, something went wrong");
            return false;
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
            }
            return OK;
        }
        public void ApplyCost (StatsEffect cost)
        {
            var cont = _stats[cost.StatType];
            if ( cont.GetCurrent >= Mathf.Abs(cost.InitialValue))
            {
                cont.ApplyStatsEffect(cost);
            }
            else
            {
                Debug.LogError($"tried to apply cost {cost} in {Owner} without checking if its possible");
            }
        }

        public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetStatValues => _stats;
        public StatValueContainer GetStatValue(BaseStatType type)
        {
            try
            {
                return _stats[type];
            }
            catch
            {
                return null;
            }
        }
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