using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Skills
{
    [RequireComponent(typeof(SphereCollider))]
    public class SkillProjectileComponent : ProjectileComponent

    {

        private SerizalizedSkillConfiguration _cfg;
        public virtual void Setup(SerizalizedSkillConfiguration cfg, BaseUnit owner)
        {
            _cfg = cfg;
            GetEffects = new EffectsCollection(cfg.Effects);
            base.Setup(cfg.SkillProjectileConfig,owner);

            SpawnPlace = cfg.SpawnPlace; // override proj object ettings
        }

        public EffectsCollection GetEffects { get; private set; }

        public SerializedStatTriggerConfig[] GetEffectCfgs => _cfg.Triggers;


        protected SkillState CurrentState = SkillState.Placer;
        public SimpleEventsHandler<Collider, SkillState> TriggerEnterEvent;
        public SimpleEventsHandler<SkillProjectileComponent> SkillDestroyedEvent;
        protected SphereCollider _collider;


        public void AdvanceStage()
        {
            CurrentState += 1;
            _framesAoe = 0;
            _collider.enabled = false;

            _collider.radius = _cfg.AoERadius;

            _collider.enabled = true; // this should trigger a second collision
            gameObject.name = "Effect" + _cfg.Description.Title;
        }
        private void OnTriggerEnter(Collider other)
        {
            //_framesAoe = 0;
            TriggerEnterEvent?.Invoke(other, CurrentState);
        }

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;


        }
        private void Start()
        {
            gameObject.name = "Placer" + _cfg.Description.Title;
            _collider.radius = _cfg.PlacerRadius;
        }

        public override void OnDestroy()
        {
            SkillDestroyedEvent?.Invoke(this);
            base.OnDestroy();
        }

        private int _framesAoe = 0;

        private void Update()
        {
            if (CurrentState == SkillState.AoE)
            {
                _framesAoe++;
            }
            if (_framesAoe >= 5)
            {
                Destroy(gameObject);
            }
        }


        #region gizmo


        #endregion


    }







}