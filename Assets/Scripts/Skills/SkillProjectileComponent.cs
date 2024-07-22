using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Skills
{
    
    public class SkillProjectileComponent : ProjectileComponent    {



        protected SkillState CurrentState = SkillState.Placer;
        public TriggerTargetType ActivatingTrigger { get; set; }
        [HideInInspector]public float SkillAreaOfEffect;
        SphereCollider _aoe;

        private void Start()
        {
            name = "Placer " + name;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            switch (CurrentState)
            {
                case SkillState.Placer:
                    if (other.CompareTag("SolidItem")) // might happen with ranged projectiles
                    {
                        Advance();
                    }
                    else if (other.TryGetComponent<DummyUnit>(out DummyUnit hit))
                    {
                        switch (ActivatingTrigger)
                        {
                            case TriggerTargetType.TargetsEnemies:
                                if (Owner.Side != hit.Side)
                                {
                                    Advance();
                                }
                                break;
                            case TriggerTargetType.TargetsUser:
                                if (Owner == hit)
                                {
                                    Advance();
                                }
                                break;
                            case TriggerTargetType.TargetsAllies:
                                if (Owner == hit || hit.Side == Owner.Side)
                                {
                                    Advance();
                                }
                                break;
                        }
                    }
                    break;
                case SkillState.AoE:
                    Advance();
                    break;
            }
        }
        void Advance()
        {
            RemainingHits = 0;

            switch (CurrentState)
            {
                case SkillState.Placer:
                    CurrentState = SkillState.AoE;
                    Debug.Log($"skill {name} collide");

                    name = "Effect " + name;
                    _aoe = gameObject.AddComponent<SphereCollider>();
                    _aoe.isTrigger = true;
                    _aoe.radius = SkillAreaOfEffect;

                   // EventBus<VFXRequest>.Raise(new VFXRequest(VFX.TryGetEffect(EffectMoment.OnCollision), transform));

                    break;
                case SkillState.AoE:
                    Debug.Log($"register aoe hit in {this}");
                    Destroy(gameObject, 0.1f);
                    break;
            }
            if (RemainingHits == 0)
            {
                Speed = 0;
            }
        }

    }







}