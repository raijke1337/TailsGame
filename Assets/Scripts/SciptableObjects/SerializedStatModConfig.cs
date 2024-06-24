
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New Serialized Stat Mod", menuName = "Configurations/Stats/Stat mod", order = 1)]
    public class SerializedStatModConfig : ScriptableObject
    {
        public Sprite Icon;
        public BaseStatType ChangedStat;
        public int InitialValue; // for general increase
        public int OverTimeValue; // for regen

    }

}