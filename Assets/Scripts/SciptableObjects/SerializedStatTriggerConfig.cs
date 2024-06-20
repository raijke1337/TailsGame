using Arcatech.Effects;
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New SerializedStatTriggerConfig", menuName = "Configurations/BaseStatTrigger", order = 1)]
    public class SerializedStatTriggerConfig : ScriptableObjectID
    {
        public TriggerChangedValue ChangedValueType;
        public int InitialValue;
        public int OverTimeValue;
        public int OverTimeDuration;
        public int OverTimeInstances;
        public Sprite Icon;

        public TriggerTargetType TargetType;

        public SerializedEffectsCollection Effects;


    }

}