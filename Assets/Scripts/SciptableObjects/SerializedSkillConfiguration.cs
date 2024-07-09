using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Skill Config", menuName = "Skills/Self Skill")]
    public class SerializedSkillConfiguration : ScriptableObject
    {

        [Header("Text")]public ExtendedText Description;
        [Space] public SerializedProjectileConfiguration Projectile;

        [Space,Header("Combat action")]public UnitActionType UnitActionType;
        public TriggerTargetType ActivatingTrigger;
        public SerializedStatsEffectConfig CostTrigger;
        public SerializedStatsEffectConfig[] Triggers;

        [Range(1, 10)] public int Charges;
        [Range(1, 10)] public int ChargeRestore;
        [Range (1,100)]public float AoE;

        [Header("Effects")]
        public SerializedEffectsCollection Effects;

        public SkillProjectileComponent ProduceProjectile(DummyUnit owner, Transform place, SerializedStatsEffectConfig[] effects)
        {
            var proj = Projectile.ProduceProjectile(owner, place, effects);
            if (proj is SkillProjectileComponent sk)
            {
                sk.ActivatingTrigger = ActivatingTrigger;
                sk.SkillAreaOfEffect = AoE;
                sk.VFX = new EffectsCollection(Effects);
                return sk;
            }
            else return null;

        }


    }
}