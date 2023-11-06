using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "New DodgeStatsConfig", menuName = "Configurations/DodgeController")]
public class DodgeStatsConfig : ScriptableObjectID
{
    public SerializedDictionary<DodgeStatType, StatValueContainer> Stats;
}

