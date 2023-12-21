using Arcatech.Effects;
using CartoonFX;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Arcatech.Managers
{
    public class EffectsManager : MonoBehaviour
    {
        #region singleton
        public static EffectsManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        #endregion
        private void Start()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            PlayMusic(GameManager.Instance.GetCurrentLevelData.Music);
        }

        [SerializeField] private AudioSource _audioPrefab;
        [SerializeField] private AudioSource _musicPrefab;
        private AudioSource _musicObj;


        public void ServeEffectsRequest(EffectRequestPackage pack)
        {
            var place = pack.Place;

            if (pack.Collection.Sounds.TryGetValue(pack.Type, out var c) && c.Sounds.Length > 0)
            {
                PlaceSound(c.Sounds[Random.Range(0, c.Sounds.Length - 1)], c.Loudness, place);
            }
            if (pack.Collection.Effects.TryGetValue(pack.Type, out var cont) && cont.Effects.Length > 0)
            {
                PlaceParticle(cont.Effects[Random.Range(0, cont.Effects.Length - 1)], cont.Scale, place, cont.Duration);
            }
        }


        private void PlaceSound(AudioClip clip, float volumeMult, Transform place)
        {
            var s = Instantiate(_audioPrefab, place.position, Quaternion.identity, transform);
            s.clip = clip;
            s.volume *= volumeMult;
            s.Play();

            Destroy(s.gameObject, s.clip.length);
        }
        private void PlaceParticle(CFXR_Effect eff, Vector3 scale, Transform place, float time)
        {
            var p = Instantiate(eff, place.position, place.rotation);
            p.transform.localScale = scale;
            if (time != 0) // 0 means it will play once and disappear
            {
                p.Animate(time);

            }
        }

        private void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            _musicObj = Instantiate(_musicPrefab);
            _musicObj.clip = clip;
            _musicObj.loop = true;
            _musicObj.Play();
        }


        public void CleanUpOnSceneChange()
        {
            StopAllCoroutines();
            if (_musicObj != null) Destroy(_musicObj.gameObject);
        }

    }
}