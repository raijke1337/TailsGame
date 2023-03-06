using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Shield")]
public class ShieldSettings : ScriptableObjectID
{

    public SerializableDictionaryBase<ShieldStatType, StatValueContainer> Stats;

}

