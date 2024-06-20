using Arcatech.Effects;
using System;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class TriggeredEffect
    {
        public string ID { get; }
        public TriggerChangedValue StatType { get; }
        public float OverTimeValue { get; set; }
        public int OverTimeDuration { get; }
        public int OverTimeInstances { get; }


        public float InitialValue { get; set; }
        public float TimeSinceTick { get; set; }

        
        public Sprite Icon { get; }
        public TriggerTargetType Target { get; }

        public float TimeToTick { get
            {
                return OverTimeDuration / OverTimeInstances;
            }
        }
        public float ValuePerTick
        {
            get
            {
                return OverTimeValue / OverTimeInstances;
            }
        }
        public EffectsCollection GetEffects { get; }

        public TriggeredEffect(SerializedStatTriggerConfig config)
        {
            ID = config.ID;
            StatType = config.ChangedValueType;
            InitialValue = config.InitialValue;
            OverTimeValue = config.OverTimeValue;
            OverTimeDuration = config.OverTimeDuration;
            OverTimeInstances = config.OverTimeInstances;
            TimeSinceTick = 0;
            Icon = config.Icon;
            Target = config.TargetType;
            GetEffects = new EffectsCollection(config.Effects);
        }
    }

}