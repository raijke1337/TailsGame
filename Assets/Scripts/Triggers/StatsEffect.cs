using Arcatech.Effects;
using System;
namespace Arcatech.Triggers
{

    public class StatsEffect : StatsMod
    {
        public StatsEffect(SerializedStatsEffectConfig cfg) : base(cfg)
        {
            OverTimeDuration = cfg.EffectDuration;
            Target = cfg.TargetType;
            GetEffects = new EffectsCollection(cfg.Effects);
            RemainingTime = OverTimeDuration;
        }

        public int OverTimeDuration { get; }
        public float RemainingTime;

       // public float TimeSinceTick { get; set; }

        public TriggerTargetType Target { get; }

        public EffectsCollection GetEffects { get; }

        public override bool CheckCondition
        {
            get
            {
                return RemainingTime > 0;
            }
        }
    }

}