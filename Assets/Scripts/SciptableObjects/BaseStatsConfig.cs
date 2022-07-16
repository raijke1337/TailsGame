using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseStatsConfig", menuName = "Configurations/Stats", order = 1)]
public class BaseStatsConfig : ScriptableObjectID
{
    public string displayName;
    public SerializableDictionaryBase<BaseStatType, StatValueContainer> Stats;
}

