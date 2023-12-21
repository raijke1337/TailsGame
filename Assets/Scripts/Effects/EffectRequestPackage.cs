using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Effects
{
    public class EffectRequestPackage
    {
        public EffectsCollection Collection { get; }
        public EffectMoment Type { get; }
        public Transform Place { get; }
        public Transform Parent; // used to destroy persistent particles, e.g. for booster trail - NYI TODO
        public EffectRequestPackage(EffectsCollection collection, EffectMoment type, Transform place)
        {
            Collection = collection;
            Type = type;
            Place = place;
        }



    }
}