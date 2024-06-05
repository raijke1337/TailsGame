using Arcatech.Triggers;
using Arcatech.Units.Stats;
using AYellowpaper.SerializedCollections;
using System;
using System.Linq;
using UnityEditor.Compilation;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class BaseStatsController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {
        public SerializedDictionary<BaseStatType, StatValueContainer> GetBaseStats { get; private set; }
       public event SimpleEventsHandler UnitDiedEvent;
       public event SimpleEventsHandler<float> UnitTookDamageEvent; //value
        public string GetDisplayName { get; }
        public override string GetUIText { get => GetDisplayName; }

        private StatValueContainer _healthContainer;

        #region ihandler
        public override void SetupStatsComponent()
        {
            foreach (var v in GetBaseStats.Values) { v.Setup(); }
            IsReady = true;
        }
        public override void StopStatsComponent()
        {
            base.StopStatsComponent();
            _healthContainer.ValueChangedEvent -= OnHealthValueChange;
        }
        #endregion


        public BaseStatsController(BaseUnit owner) : base(owner)
        {
            GetBaseStats = new SerializedDictionary<BaseStatType, StatValueContainer>();
            var cfg = owner.StatsConfig;

            if (cfg == null) return;


            var _keys = cfg.Stats.Keys.ToArray();
            var _values = cfg.Stats.Values.ToArray();

            for (int i = 0; i < _keys.Count(); i++)
            {
                GetBaseStats.Add(_keys[i], new StatValueContainer(_values[i]));
            }

            GetDisplayName = cfg.displayName;
            _healthContainer = GetBaseStats[BaseStatType.Health];
            _healthContainer.ValueChangedEvent += OnHealthValueChange;

        }
        protected void OnHealthValueChange(float current, float prev)
        {
            if (!IsReady) return;
            Debug.Log($"basestatsctrl: hp change {prev} -> {current}");
            if (prev>current)
            {
                UnitTookDamageEvent?.Invoke(prev - current);
            }
            if (current <= 0)
            {
                Debug.Log($"basestatsctrl: dead event in {GetDisplayName}");
                UnitDiedEvent?.Invoke();
            }
        }



        // numbers change is all here       
        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            StatValueContainer result = null;
            switch (effect.StatType)
            {
                case TriggerChangedValue.Health:
                    result = GetBaseStats[BaseStatType.Health];
                    break;
                case TriggerChangedValue.MoveSpeed:
                    result = GetBaseStats[BaseStatType.MoveSpeed];
                    break;
                case TriggerChangedValue.TurnSpeed:
                    result = GetBaseStats[BaseStatType.TurnSpeed];
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}