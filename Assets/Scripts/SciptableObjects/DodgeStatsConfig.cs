
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Arcatech
{
    [CreateAssetMenu(fileName = "New DodgeStatsConfig", menuName = "Configurations/DodgeController")]
    public class DodgeStatsConfig : ScriptableObjectID
    {
        public SerializedDictionary<DodgeStatType, StatValueContainer> Stats;
    }

}