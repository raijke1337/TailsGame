using Arcatech.Effects;
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New SerializedStatTriggerConfig", menuName = "Configurations/BaseStatTrigger", order = 1)]
    public class SerializedStatTriggerConfig : ScriptableObjectID
    {
        public TriggerChangedValue ChangedValueType;
        public float InitialValue;
        public float RepeatedValue;
        public float RepeatApplicationDelay;
        public float TotalDuration;
        public Sprite Icon;

        public TriggerTargetType TargetType;

        public SerializedEffectsCollection Effects;


    }

}