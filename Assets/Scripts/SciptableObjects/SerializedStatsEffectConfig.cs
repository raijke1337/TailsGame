using Arcatech.Effects;
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New Serialized Stats change effect", menuName = "Actions/Stat Change trigger cfg")]
    public class SerializedStatsEffectConfig : SerializedStatModConfig
    {

        public int EffectDuration;

        public TriggerTargetType TargetType;

        public SerializedEffectsCollection Effects;

    }

}