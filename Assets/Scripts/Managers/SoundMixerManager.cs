using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    // call in game settings
    public void SetMaster(float level)
    {
        _mixer.SetFloat("VolMaster", Mathf.Log10(level) * 20);
        // this is because mixer used decibels and they adjust in a log scale
        // TODO volume settings sliders values must be 0.00001 to 1
    }
    public void SetFX(float level)
    {
        _mixer.SetFloat("VolFX", level);
    }
    public void SetMusic(float level)
    {
        _mixer.SetFloat("VolMusic", level);
    }

    public float GetSFXVolume { get
        {
            _mixer.GetFloat("VolFX", out float f);
            return f;
        }
    }
    // todo save to playerprefs

}
