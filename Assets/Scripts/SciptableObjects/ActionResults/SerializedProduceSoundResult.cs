﻿using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Managers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New play sound result ", menuName = "Actions/Action Result/Produce sound effects")]
    public class SerializedProduceSoundResult : SerializedActionResult
    {
       
        [SerializeField] SoundClipData[] sounds;
        [SerializeField] bool RandomPitch = false;
        public override IActionResult GetActionResult()
        {
            return new ProduceSoundResult(sounds,RandomPitch);
        }
    }


    public class ProduceSoundResult : ActionResult
    {
        readonly SoundClipData[] sounds;
        bool pitch;
        public ProduceSoundResult(SoundClipData[] d, bool pitch)
        {
            sounds = d;
            this.pitch = pitch;
        }

        public override void ProduceResult(BaseEntity user, BaseEntity target, Transform place)
        {
            foreach (var s in sounds)
            {
                EventBus<SoundClipRequest>.Raise(new SoundClipRequest(s,pitch,user.transform.position));
            }
        }
    }




}