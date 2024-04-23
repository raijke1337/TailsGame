using Arcatech.Triggers;
using Arcatech.Units.Stats;
using AYellowpaper.SerializedCollections;
using System;
using System.Linq;

namespace Arcatech.Units
{
    [Serializable]
    public class BaseStatsController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {
        public SerializedDictionary<BaseStatType, StatValueContainer> GetBaseStats { get; private set; }
        public event SimpleEventsHandler UnitDiedEvent;
        public string GetDisplayName { get; }
        public override string GetUIText { get => GetDisplayName; }

        #region ihandler
        public override void SetupStatsComponent()
        {
            foreach (var v in GetBaseStats.Values) { v.Setup(); }
        }
        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);
            if (GetBaseStats[BaseStatType.Health].GetCurrent <= 0f)
            {
                UnitDiedEvent?.Invoke();
            }
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
            IsReady = true;
        }


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