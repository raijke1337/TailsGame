using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Skill Config", menuName = "Skills/Self Skill")]
    public class SerizalizedSkillConfiguration : ScriptableObject
    {

        public ExtendedText Description;
        public EquipmentType SpawnPlace;
        [Space, Range(1, 10)] public int Charges;
        [Range(1, 10)] public int ChargeRestore;
        [Space] public SerializedStatTriggerConfig CostTrigger;

        [Space] public SerializedProjectileConfiguration SkillProjectileConfig;

        [Range(0.1f, 10f)] public float PlacerRadius;
        [Range(0.1f, 10f)] public float AoERadius;
        [Space, Range(0.01f, 5f)] public float AoETime;

        [Space] public SerializedStatTriggerConfig[] Triggers;
        [Space] public SerializedEffectsCollection Effects;


    }
}