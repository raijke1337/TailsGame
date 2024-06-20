
using Arcatech.Stats;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Arcatech.Units.Stats
{
    [CreateAssetMenu(fileName = "New BaseStatsConfig", menuName = "Configurations/Stats", order = 1)]
    public class BaseStatsConfig : ScriptableObjectID
    {
        public string displayName;
        public SerializedDictionary<BaseStatType, StatValueContainer> Stats;
    }

}