using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Stats
{
    [Serializable]
    public class EnergyController : IManagedComponent
    {
        #region serialized
        [SerializeField] private string _value;
        #endregion


        private Dictionary<GeneratorStatType, StatValueContainer> _shieldStats;

       // public event SimpleEventsHandler<bool, IManagedComponent> ComponentChangedStateToEvent;

        public EnergyController(GeneratorSettings settings)
        {
            _shieldStats = new Dictionary<GeneratorStatType, StatValueContainer>();
            foreach (KeyValuePair<GeneratorStatType, StatValueContainer> p in settings.Stats)
            {
                _shieldStats[p.Key] = new StatValueContainer(p.Value);
                _shieldStats[p.Key].StartComp();
            }
            StartComp();
        }


        public StatValueContainer GetValueContainer { get { return _shieldStats[GeneratorStatType.Capacity]; } }

        public bool IsReady { get; private set; }
        public bool TryAbsorb (TriggeredEffect incoming, out TriggeredEffect remaining)
        {
            remaining = incoming;
            if (!IsReady || _shieldStats[GeneratorStatType.Capacity].GetCurrent == 0) return false;
            else
            {
                float mult = _shieldStats[GeneratorStatType.DamageAbsorbMultiplier].GetCurrent;

                incoming.InitialValue *= mult;
                incoming.OverTimeValue *= mult;


                if (incoming.InitialValue <= _shieldStats[GeneratorStatType.Capacity].GetCurrent)
                {
                    incoming.OverTimeValue = 0;
                    _shieldStats[GeneratorStatType.Capacity].ApplyTriggeredEffect(incoming);
                    return true;
                }
                else
                {
                    float absorb = _shieldStats[GeneratorStatType.DamageAbsorbMultiplier].GetCurrent;
                    incoming.OverTimeValue = 0;
                    _shieldStats[GeneratorStatType.Capacity].ApplyTriggeredEffect(incoming);

                    remaining.InitialValue -= absorb;
                    return false;
                }
            }
        }

        public void UpdateInDelta(float deltaTime)
        {
            if (!IsReady) return;

            foreach (var st in _shieldStats)
            {
                st.Value.UpdateInDelta(deltaTime);
            }
        }

        public void StartComp()
        {
            IsReady = true;
        }

        public void StopComp()
        {
            IsReady = false;
        }
    }
}