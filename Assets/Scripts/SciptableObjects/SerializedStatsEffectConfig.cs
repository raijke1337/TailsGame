using Arcatech.Actions;
using Arcatech.Effects;
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New Serialized Stats change effect", menuName = "Actions/Stat Change trigger cfg")]
    public class SerializedStatsEffectConfig : ScriptableObject
    {
        public int Hash { get => GetHashCode(); }
        public BaseStatType ChangedStat;
        public int InitialValue; // value change

        public int OverTimeValue; // how much dot or hot will be done
        public int OverTimeValueDuration; // over how much time
        public SerializedActionResult OnApplyResult;
    }

}