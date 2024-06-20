using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Stats
{

    [Serializable]
    public class StatValueContainer : IManagedComponent
    {
        [SerializeField] private float _start;
        [SerializeField] private float _max;
        [SerializeField] private float _min;

        private float _current;
        private float _previous;
        private bool _isSetup = false;


        /// <summary>
        /// current, previous
        /// </summary>
        public SimpleEventsHandler<float, float> ValueChangedEvent;

        public event SimpleEventsHandler<bool, IManagedComponent> ComponentChangedStateToEvent;

        public float GetCurrent { get => _current; }
        public float GetMax { get => _max; }
        public float GetMin { get => _min; }

        public bool IsReady => _isSetup;
        public bool HasCapacity(float value)
        {
            return _current >= value;
        }

        private List<TriggeredEffect> _currentEffects;

        public void StartComp()
        {
            _current = _start;
            if (_max == 0) _max = _start;
            _currentEffects = new List<TriggeredEffect>();
            _previous = _current;
            _isSetup = true;
        }

        private float reportCooldown; // to not spam update with micro hp changes

        public void UpdateInDelta(float deltaTime)
        {
            if (_isSetup)
            {
                reportCooldown += deltaTime;
                foreach (var eff in _currentEffects)
                {
                    eff.TimeSinceTick += deltaTime;
                    if (eff.TimeSinceTick > eff.TimeToTick)
                    {
                        eff.TimeSinceTick = 0;
                        _current = Mathf.Clamp(_current + eff.ValuePerTick, _min, _max);
                    }
                }
                if (reportCooldown > 0.33f)
                {
                    reportCooldown = 0;
                    ValueChangedEvent?.Invoke(_current, _previous);
                    _previous = _current;
                }
            }
        }
        public void StopComp()
        {

        }
        public void ApplyTriggeredEffect(TriggeredEffect eff)
        {
            _previous = _current;

            if (eff.OverTimeDuration == 0) // instant effect
            {
                _current = Mathf.Clamp(_current + eff.InitialValue, _min, _max);
            }
            else
            {
                eff.InitialValue = 0;
                _currentEffects.Add(eff);
            }

            ValueChangedEvent?.Invoke(_current, _previous);
        }
        public override string ToString()
        {
            return ($"{Mathf.RoundToInt(GetCurrent)} / {Mathf.RoundToInt(GetMax)}");
        }

        public StatValueContainer(StatValueContainer preset)
        {
            _start = preset._start;
            _max = preset._max;
            _min = preset._min;
        }
    }
}