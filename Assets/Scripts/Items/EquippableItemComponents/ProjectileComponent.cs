using Arcatech.Actions;
using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class ProjectileComponent : MonoBehaviour
    {
        public BaseEntity Owner { get; set; }
        [HideInInspector] public int RemainingHits;
        [HideInInspector] public float Lifetime;
        [HideInInspector] public float Speed;

        public SerializedEffectsCollection VFX;
        EffectsCollection _fx;

        IActionResult[] UnitCollisionResult; // explode (place aoe projectile) or apply effects
        IActionResult[] ExpirationCollisionResult; // explode (place aoe projectile) or stop moving


        public void SetResult(SerializedActionResult[] cfg, SerializedActionResult[] exp)
        {
            UnitCollisionResult = new ActionResult[cfg.Length];
            for (int i = 0; i < UnitCollisionResult.Length; i++)
            {
                UnitCollisionResult[i] = cfg[i].GetActionResult();
            }

            ExpirationCollisionResult = new ActionResult[exp.Length];
            for (int i = 0; i < ExpirationCollisionResult.Length; i++)
            {
                ExpirationCollisionResult[i] = exp[i].GetActionResult();
            }
        }
        private void Start()
        {
            _fx = new EffectsCollection(VFX);
            if (_fx.TryGetEffect(EffectMoment.OnStart, out var e))
            {
                EventBus<VFXRequest>.Raise(new VFXRequest(e, transform));
            }
        }
        private void Update()
        {
            transform.position += Speed * Time.deltaTime * transform.forward;
            Lifetime -= Time.deltaTime;
            if (Lifetime < 0)
            {
                Destroy(gameObject);
            }
        }


        protected void OnDestroy()
        {
            if (_fx.TryGetEffect(EffectMoment.OnExpiry, out var e))
            {
                EventBus<VFXRequest>.Raise(new VFXRequest(e, transform));
            }
            if (ExpirationCollisionResult.Length > 0)
            {
                foreach (var exp in ExpirationCollisionResult)
                {
                    exp.ProduceResult(Owner,null,transform);
                }
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{this} hit {other.gameObject}!");

            if (other.TryGetComponent<BaseEntity>(out var u))
            {
                // hit an entioty
                if (u != Owner && u.Side != Owner.Side)
                {
                    RemainingHits--;

                    if (UnitCollisionResult.Length > 0)
                    {
                        foreach (var uc in UnitCollisionResult)
                        {
                            uc.ProduceResult(Owner, u, transform);
                        }
                    }
                    if (_fx.TryGetEffect(EffectMoment.OnCollision, out var e))
                    {
                        EventBus<VFXRequest>.Raise(new VFXRequest(e, transform));
                    }
                }
            }
            if (other.CompareTag("SolidItem") || other.gameObject.isStatic)
            {
                RemainingHits = 0;
            }

            if (RemainingHits == 0)
            {
                Destroy(gameObject);
            }
        }


    }
}
