using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New sounds card", menuName = "Configurations/Sounds Card")]
public class AudioCardSO : ScriptableObjectID
{
    public SerializableDictionaryBase<SoundType, AudioClip> SoundsDict;
}
