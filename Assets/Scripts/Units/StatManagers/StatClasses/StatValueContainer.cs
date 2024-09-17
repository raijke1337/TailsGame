
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Stats
{
    [Serializable]
    public class StatValueContainer
    {
        #region public 
        public float GetCurrent { get => _currentValue; }
        public float GetMax { get => _maxValue; }
        public float GetMin { get => _minValue; }
        public float GetPercent { get => _currentValue / _maxValue; }
        public float CachedValue { get; set; } // to store changes between updates
        #endregion

        private float _currentValue;
        private float _maxValue;
        private float _minValue = 0f;
        private bool _setup = false;

        public override string ToString()
        {
            return ($"{Mathf.RoundToInt(GetCurrent)} / {Mathf.RoundToInt(GetMax)}");
        }

        public StatValueContainer()
        {
            _currentEffects = new List<StatsEffect>();
            _currentMods = new();
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
            HandleMods(deltaTime);
            HandleEffects(deltaTime);
        }

        #region mods
        private List<StatsMod> _currentMods;
        public void ApplyStatsMod(StatsMod mod)
        {
            _maxValue += mod.InitialValue;
            _currentMods.Add(mod);
        }
        void HandleMods(float d)
        {
            foreach (var mod in _currentMods.ToList())
            {
                if (!mod.CheckCondition(d))
                {
                    _maxValue-=mod.InitialValue;
                    _currentMods.Remove(mod);
                }
            }
        }

        #endregion

        #region temporary effects
        private List<StatsEffect> _currentEffects;

        public void ApplyStatsEffect(StatsEffect eff)
        {
            _currentValue = Mathf.Clamp(_currentValue + eff.InitialValue, _minValue, _maxValue);
            _currentEffects.Add(eff);
        }

        void HandleEffects(float d)
        {
            foreach (var eff in _currentEffects.ToList())
            {
                if (!eff.CheckCondition(d))
                {
                    _currentEffects.Remove(eff); // has no duration or is expired
                }
                else
                {
                    _currentValue = Mathf.Clamp(_currentValue + eff.FrameDelta, _minValue, _maxValue);
                }
            }

        }

        #endregion
    }
}