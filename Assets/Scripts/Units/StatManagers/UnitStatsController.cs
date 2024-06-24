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
    public class UnitStatsController : MonoBehaviour, IManagedComponent 
    {
        public Dictionary<BaseStatType, StatValueContainer> CurrentStats { get; set;  }

        public bool IsReady => throw new NotImplementedException();

        private EnergyController _energy;
        public event SimpleEventsHandler ZeroHealthEvent; // dead
        public event SimpleEventsHandler StunEvent; // just stun
        public event SimpleEventsHandler KnockDownEvent; // fall, armor damage
        public event SimpleEventsHandler<float> UnitTookDamageEvent; //        


        //public UnitStatsController(ItemEmpties empties, BaseUnit owner) : base(empties, owner)
        //{
        //    CurrentStats = new Dictionary<BaseStatType, StatValueContainer>();
        //}

        #region ihandler


        #endregion

        public StatValueContainer AssessStat(TriggerChangedValue stat)
        {
            switch (stat)
            {
                case TriggerChangedValue.Health:
                    return CurrentStats[BaseStatType.Health];
                case TriggerChangedValue.Energy:
                    return _energy.GetValueContainer;
                case TriggerChangedValue.Stamina:
                    return CurrentStats[BaseStatType.Stamina];
                case TriggerChangedValue.MoveSpeed:
                    return CurrentStats[BaseStatType.MoveSpeed];
                default:
                    Debug.LogError($"Tried to assess stat {stat}, not implemented");
                    break;
            }
            return null;

        }

        protected void OnHealthValueChange(float current, float prev)
        {
            if (!IsReady) return;
            if (prev>current)
            {
                UnitTookDamageEvent?.Invoke(prev - current);
            }
            if (current <= 0)
            {                
                ZeroHealthEvent?.Invoke();
            }
        }


        public void ApplyEffect(TriggeredEffect effect)
        {
            switch (effect.StatType)
            {
                case TriggerChangedValue.Health:
                    if (!_energy.TryAbsorb(effect,out var remaining))
                    {
                        CurrentStats[BaseStatType.Health].ApplyTriggeredEffect(remaining);
                    }
                    break;
                case TriggerChangedValue.Energy:
                    _energy.GetValueContainer.ApplyTriggeredEffect(effect);
                    break;
                case TriggerChangedValue.Stamina:
                    CurrentStats[BaseStatType.Stamina].ApplyTriggeredEffect(effect);
                    break;
                default:
                    Debug.LogWarning($"trigger of type {effect.StatType} not implemented");
                    break;
            }
        }


        public override void UpdateInDelta(float deltaTime)
        {
            foreach (var stat in CurrentStats)
            {
                stat.Value.UpdateInDelta(deltaTime);
            }
            if (_energy != null)
            {
                _energy.UpdateInDelta(deltaTime);
            }
        }

        public void StartComp()
        {
            throw new NotImplementedException();
        }

        public void StopComp()
        {
            throw new NotImplementedException();
        }
    }
}