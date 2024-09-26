using Arcatech.Actions;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{


    [RequireComponent(typeof(WeaponTriggerComponent))]
    public class ProjectileComponent : MonoBehaviour
    {
        public BaseEntity Owner { get; set; }
        [HideInInspector] public int RemainingHits;
        [HideInInspector] public float Lifetime;
        [HideInInspector] public float Speed;
        protected bool hasHitUnit = false;
        WeaponTriggerComponent col;
        bool isAoe = false; // bandaid but w/e

        //public SerializedEffectsCollection VFX;
       // EffectsCollection _fx;

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
            col = GetComponent<WeaponTriggerComponent>();
            if (GetComponent<AreaOfEffectSphereScalerComponent>()) isAoe = true;
            col.SomethingHitEvent += Col_SomethingHitEvent;
        }

        protected virtual void Col_SomethingHitEvent(Collider other)
        {
            if (other.TryGetComponent<BaseEntity>(out var u))
            {
                // hit an entioty
                //Debug.Log($"{this} hit {u.GetUnitName}!");
                if (u != Owner && u.Side != Owner.Side)
                {
                    hasHitUnit = true;
                    RemainingHits--;

                    if (UnitCollisionResult.Length > 0)
                    {
                        foreach (var uc in UnitCollisionResult)
                        {
                            uc.ProduceResult(Owner, u, transform);
                        }
                    }
                }
            }
            if (isAoe) return;

            if (other.CompareTag("SolidItem") || other.gameObject.isStatic)
            {
                RemainingHits = 0;
            }

            if (RemainingHits == 0)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Update()
        {
            transform.position += Speed * Time.deltaTime * transform.forward;
            Lifetime -= Time.deltaTime;
            if (Lifetime < 0)
            {
                Destroy(gameObject);
            }
        }


        protected virtual void OnDestroy()
        {

            if (ExpirationCollisionResult.Length > 0 && !hasHitUnit)
            {
                foreach (var exp in ExpirationCollisionResult)
                {
                    exp.ProduceResult(Owner,null,transform);
                }
            }
        }



    }
}
