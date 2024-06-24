using Arcatech.Effects;
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New Serialized Stats effect", menuName = "Configurations/Stats/Stat Effect", order = 2)]
    public class SerializedStatsEffectConfig : SerializedStatModConfig
    {

        public int EffectDuration;

        public TriggerTargetType TargetType;

        public SerializedEffectsCollection Effects;

    }

}