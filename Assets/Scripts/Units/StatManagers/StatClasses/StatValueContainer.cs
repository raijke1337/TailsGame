
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Stats
{
    [Serializable]
    public class StatValueContainer
    {

        private float _currentValue;
        private float _previous;
        private float _maxValue;
        private float _minValue = 0f;

        private bool _setup = false;

        public float GetCurrent { get => _currentValue; }
        public float GetMax { get => _maxValue; }
        public float GetMin { get => _minValue; }
        public float GetPercent { get => _currentValue/_maxValue; }

        private List<StatsEffect> _currentEffects;
        private List<StatsMod> _currentMods;


        public void ApplyStatsMod(StatsMod mod)
        {
            _maxValue += mod.InitialValue;
            _currentMods.Add(mod);
        }

        public void ApplyStatsEffect(StatsEffect eff)
        {
            _previous = _currentValue;

            if (eff.OverTimeDuration == 0) // instant effect
            {
                _currentValue = Mathf.Clamp(_currentValue + eff.InitialValue, _minValue, _maxValue);
            }
            else
            {
                eff.InitialValue = 0;
                _currentEffects.Add(eff);
            }

        }
        public override string ToString()
        {
            return ($"{Mathf.RoundToInt(GetCurrent)} / {Mathf.RoundToInt(GetMax)}");
        }

        public StatValueContainer() {
            _currentEffects = new List<StatsEffect>();
            _currentMods = new();
        }
        public StatValueContainer (SimpleContainerConfig cfg)
        {
            _maxValue = cfg.Max; _minValue = cfg.Min;
            _currentValue = cfg.Start;
            _setup = true;
        }

        public void Initialize(float startValue)
        {
            if (!_setup) 
            {
                _currentValue = Mathf.Clamp(startValue, _minValue, _maxValue);
                _setup = true;
            }

        }

        public void UpdateInDelta(float deltaTime)
        {
            if (!_setup) return;
            _previous = _currentValue;

            foreach (var mod in  _currentMods)
            {
                if (mod.CheckCondition)

                {
                    _currentValue = Mathf.Clamp(_currentValue + (mod.PerSecondChange * deltaTime),_minValue,_maxValue);
                }
            }
            foreach (var eff in _currentEffects)
            {
                if (eff.CheckCondition)
                {
                    _currentValue += Mathf.Clamp(_currentValue + (eff.PerSecondChange * deltaTime),_minValue,_maxValue);
                    eff.RemainingTime -= deltaTime;
                }
            }
            
        }
    }
}