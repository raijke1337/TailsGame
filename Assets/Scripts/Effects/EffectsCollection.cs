using AYellowpaper.SerializedCollections;
using CartoonFX;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Effects
{
    [Serializable]
    public class SerializedEffectsCollection
    {
        [SerializeField] public SerializedDictionary<EffectMoment, AudioClip[]> Sounds;
        [SerializeField] public SerializedDictionary<EffectMoment, CFXR_Effect[]> Effects;
    }

    [Serializable]
    public class EffectsCollection
    {
        private Dictionary <EffectMoment, AudioClip[]> _soundsdict;
        private Dictionary<EffectMoment, CFXR_Effect[]> _effdict;
        public Transform ParentTransform { get; set; }
        public AudioClip[] GetSounds (EffectMoment m)
        {
            if (_soundsdict.TryGetValue(m, out var clips))
            {
                return clips;
            }
            else return null;
        }
        public CFXR_Effect[] GetEffects (EffectMoment m)
        {
            if (_effdict.TryGetValue(m, out var clips))
            {
                return clips;
            }
            else return null;
        }

        public AudioClip GetRandomSound(EffectMoment m)
        {
            if (_soundsdict.TryGetValue(m, out var clips))
            {
                return clips[UnityEngine.Random.Range(0,clips.Length)];
            }
            else return null;
        }
        public bool TryGetEffect (EffectMoment m, out CFXR_Effect eff)
        {
            if (_effdict.TryGetValue(m, out var clips))
            {
                eff = clips[UnityEngine.Random.Range(0,clips.Length)];
                return true;
            }
            else
            {
                eff = null;
                return false;

            }
        }


        public EffectsCollection(SerializedEffectsCollection cfg, Transform parentT = null)
        {
            _soundsdict = new Dictionary<EffectMoment, AudioClip[]>(cfg.Sounds);
            _effdict = new Dictionary<EffectMoment, CFXR_Effect[]>(cfg.Effects);
            ParentTransform = parentT;
        }





    }



}