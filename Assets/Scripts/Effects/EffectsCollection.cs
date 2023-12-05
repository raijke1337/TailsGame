using AYellowpaper.SerializedCollections;
using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Effects
{
    [Serializable]
    public class EffectsCollection
    {
        [SerializeField] public SerializedDictionary<EffectMoment,AudioClip> Sounds;
        [SerializeField] public SerializedDictionary<EffectMoment,CFXR_Effect> Effects;

    }
}