using UnityEngine;
namespace Arcatech.Managers
{
    public abstract class LoadedManagerBase : MonoBehaviour
    {
        public abstract void Initiate();
        public abstract void RunUpdate(float delta);
        public abstract void Stop();



        //protected virtual void SoundPlayCallback(AudioClip clip, Vector3 pos) => EffectsManager.Instance.PlaySound(clip, pos);

    }
}