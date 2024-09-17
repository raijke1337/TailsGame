using Arcatech.Effects;
using Arcatech.EventBus;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Arcatech.Managers
{
    public class EffectsManager : MonoBehaviour
    {
        [Header("Sound prefabs")]
        [SerializeField] private AudioSource _audioPrefab;
        [SerializeField] private AudioSource _musicPrefab;
        [Header("Effect prefabs")]
        [SerializeField] private DamageTextContainer _particleTextPrefab;
        



        private EventBinding<DrawDamageEvent> _drawDamageEventBind;
        private EventBinding<VFXRequest> _placeParticleEventBind;
        private AudioSource _musicObj;



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
            Vector3 dirToCamera = Camera.main.transform.position - @event.Unit.transform.position;
            Vector3 adjustedPosition = @event.Unit.transform.position + (Vector3.up * 2) + dirToCamera.normalized; // move towards camera 1

            var txt = Instantiate(_particleTextPrefab,adjustedPosition,Quaternion.identity);

            txt.PlayNumbers((int)@event.Damage);
        }



        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            PlayMusic(GameManager.Instance.GetCurrentLevelData.Music);
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

        private void OnDisable()
        {
            EventBus<DrawDamageEvent>.Deregister(_drawDamageEventBind);
            EventBus<VFXRequest>.Deregister(_placeParticleEventBind);
        }

    }
}