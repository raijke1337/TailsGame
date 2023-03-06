using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(fileName = "New DodgeStatsConfig", menuName = "Configurations/DodgeController")]
public class DodgeStatsConfig : ScriptableObjectID
{
    public SerializableDictionaryBase<DodgeStatType, StatValueContainer> Stats;
}

