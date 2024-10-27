using Arcatech.Effects;
using UnityEngine;

namespace Arcatech.Managers
{
    public partial class EffectsManager
    {
        public class SoundsBuilder
        {
            readonly EffectsManager efManager;
            SoundClipData sdata;
            Vector3 position = Vector3.zero;

            bool randomPitch = false;

            public SoundsBuilder (EffectsManager m) => efManager = m;
            public SoundsBuilder WithSoundData(SoundClipData data)
            {
                sdata = data;
                return this;
            }
            public SoundsBuilder WithPosition(Vector3 data)
            {
                position = data;
                return this;
            }
            public SoundsBuilder WithRandomPitch(bool data)
            {
                randomPitch = data;
                return this;
            }

            public void Play()
            {
                if (!efManager.CanPlaySound(sdata)) return;
                SoundEmitter em = efManager.GetSound();
                em.Initialize(sdata);
                em.transform.position = position;
                em.transform.SetParent(efManager.transform);

                if (randomPitch)
                {
                    em.WithRandomPitch();
                }

                if (efManager.Counts.TryGetValue(sdata, out var count))
                {
                    efManager.Counts[sdata] = count + 1;
                }
                else
                {
                    efManager.Counts[sdata] = 1;
                }
                em.Play();
            }

        }


    }
}