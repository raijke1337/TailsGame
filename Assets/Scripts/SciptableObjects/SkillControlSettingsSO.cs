using Arcatech.Effects;
using Arcatech.Triggers;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Self Skill Config", menuName = "Skills/Self Skill")]
    public class SkillControlSettingsSO : ScriptableObject
    {

        public TextContainerSO Description;
        [Space] public float Cooldown;
        [Range(0, 100)] public int Cost;

        [Space] public SkillComponent SkillObject;
        [Range(0.1f, 10f)] public float PlacerRadius;
        [Range(0.1f, 10f)] public float AoERadius;
        [Space, Range(0.01f, 5f)] public float AoETime;

        [Space] public BaseStatTriggerConfig[] Triggers;
        [Space] public EffectsCollection Effects;
    }
}