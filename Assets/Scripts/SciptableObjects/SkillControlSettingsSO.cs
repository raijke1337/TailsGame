using Arcatech.Effects;
using Arcatech.Texts;
using Arcatech.Triggers;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Skill Config", menuName = "Skills/Self Skill")]
    public class SkillControlSettingsSO : ScriptableObject
    {

        public ExtendedTextContainerSO Description;
        [Space, Range(1, 10)] public int Charges;
        [Range(1, 10)] public int ChargeRestore;
        [Space] public BaseStatTriggerConfig ComboCostTrigger;

        [Space] public SkillComponent SkillObject;
        [Range(0.1f, 10f)] public float PlacerRadius;
        [Range(0.1f, 10f)] public float AoERadius;
        [Space, Range(0.01f, 5f)] public float AoETime;

        [Space] public BaseStatTriggerConfig[] Triggers;
        [Space] public EffectsCollection Effects;
    }
}