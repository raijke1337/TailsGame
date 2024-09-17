using Arcatech.Actions;
using Arcatech.Effects;
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New Serialized Stats change effect", menuName = "Actions/Stat Change trigger cfg")]
    public class SerializedStatsEffectConfig : SerializedStatModConfig
    {
        public int OverTimeValue; // how much dot or hot will be done
        public int OverTimeValueDuration; // over how much time
        public SerializedActionResult OnApplyResult;
    }

}