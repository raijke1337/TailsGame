using AYellowpaper.SerializedCollections;
using UnityEngine;


namespace Arcatech.Audio
{
    [CreateAssetMenu(fileName = "New sounds card", menuName = "Configurations/Sounds Card")]
    public class AudioCardSO : ScriptableObjectID
    {
        public SerializedDictionary<SoundType, AudioClip> SoundsDict;
    }
}