using Arcatech.Effects;
using Arcatech.EventBus;
using CartoonFX;
using System;
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
            else { Destroy(gameObject); }
        }
        #endregion
        private void Start()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            _drawDamageEventBind = new EventBinding<DrawDamageEvent>(PlaceDamageText);
            _placeParticleEventBind = new EventBinding<VFXRequest>(PlaceParticle);

            EventBus<DrawDamageEvent>.Register(_drawDamageEventBind);
            EventBus<VFXRequest>.Register(_placeParticleEventBind);

        }

        private void PlaceParticle(VFXRequest request)
        {
            if (request.Effect == null) return;
            var ef = Instantiate(request.Effect, request.Place.position, request.Place.rotation);
            if (request.Parent != null) { ef.transform.SetParent(request.Parent.transform, true); }
        }

        private void PlaceDamageText(DrawDamageEvent @event)
        {
            Debug.Log($"Place damage text {@event.Damage} at {@event.Unit.transform.position}");
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            PlayMusic(GameManager.Instance.GetCurrentLevelData.Music);
        }




        [Header("Sound prefabs")]
        [SerializeField] private AudioSource _audioPrefab;
        [SerializeField] private AudioSource _musicPrefab;
        [Header("Effect prefabs")]
        [SerializeField] private CFXR_ParticleText _particleTextPrefab;
        private EventBinding<DrawDamageEvent> _drawDamageEventBind;       
        private EventBinding<VFXRequest> _placeParticleEventBind;       

        private AudioSource _musicObj;


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

        private void OnDisable()
        {

            EventBus<DrawDamageEvent>.Deregister(_drawDamageEventBind);

            EventBus<VFXRequest>.Deregister(_placeParticleEventBind);
        }

    }
}