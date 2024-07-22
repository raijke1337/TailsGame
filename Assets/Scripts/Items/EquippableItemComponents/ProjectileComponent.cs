using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class ProjectileComponent : MonoBehaviour
    {
        public BaseUnit Owner { get; set; }
        [HideInInspector] public int RemainingHits;
        [HideInInspector] public float Lifetime;
        [HideInInspector] public float Speed;
        protected StatsEffect[] Effects;
        public SerializedEffectsCollection VFX;
        EffectsCollection _fx;

        public void AddEffects(SerializedStatsEffectConfig[] cfgs)
        {
            Effects = new StatsEffect[cfgs.Length];
            for (int i = 0; i < Effects.Length; i++)
            {
                Effects[i] = new StatsEffect(cfgs[i]);
            }
        }

        private void Update()
        {
            transform.position += Speed * Time.deltaTime * transform.forward;
            Lifetime -= Time.deltaTime;

            if ( Lifetime < 0 )
            { Destroy(gameObject); }

        }
        private void OnEnable()
        {
            _fx = new EffectsCollection(VFX);

        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<DummyUnit>(out var u) && u != Owner)
            {
                EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(u,Owner,true,transform,Effects));

                RemainingHits--;
                if (_fx.TryGetEffect(EffectMoment.OnCollision, out var e))
                {
                    EventBus<VFXRequest>.Raise(new VFXRequest(e, transform));
                }
                if (RemainingHits == 0) { transform.parent = other.transform; }
            }
            if (other.CompareTag("SolidItem"))
            {
                if (_fx.TryGetEffect(EffectMoment.OnExpiry, out var e))
                {
                    EventBus<VFXRequest>.Raise(new VFXRequest(e, transform));
                }
                RemainingHits = 0;
            }
            if (RemainingHits == 0)
            {
                if (_fx.TryGetEffect(EffectMoment.OnExpiry, out var e))
                {
                    EventBus<VFXRequest>.Raise(new VFXRequest(e, transform));
                }
                Speed = 0;
            }
        }

    }
}
