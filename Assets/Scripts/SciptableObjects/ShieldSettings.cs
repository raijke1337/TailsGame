
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Shield")]
public class ShieldSettings : ScriptableObjectID
{

    public SerializedDictionary<ShieldStatType, StatValueContainer> Stats;

}

