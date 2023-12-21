using AYellowpaper.SerializedCollections;
using CartoonFX;
using System;
using UnityEngine;
namespace Arcatech.Effects
{
    [Serializable]
    public class EffectsCollection
    {
        [SerializeField] public SerializedDictionary<EffectMoment, AudioContainer> Sounds;
        [SerializeField] public SerializedDictionary<EffectMoment, CFXR_Container> Effects;
    }

    [Serializable]
    public class AudioContainer
    {
        public AudioClip[] Sounds;
        [Range(1, 0)] public float Loudness;
    }
    [Serializable]
    public class CFXR_Container
    {
        public CFXR_Effect[] Effects;
        [SerializeField, Range(1, 0)] private float _scale;
        public Vector3 Scale { get => new Vector3(_scale, _scale, _scale); }
        public float Duration;
    }


}