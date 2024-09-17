
using UnityEngine;
namespace Arcatech.Triggers
{
    [CreateAssetMenu(fileName = "New Serialized Stat Mod", menuName = "Items/Stats/Stat mod", order = 1)]
    public class SerializedStatModConfig : ScriptableObject
    {
        public int Hash { get => GetHashCode(); }
        public BaseStatType ChangedStat;
        public int InitialValue; // value change

    }

}