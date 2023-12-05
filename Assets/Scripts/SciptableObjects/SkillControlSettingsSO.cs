using Arcatech.Effects;
using Arcatech.Triggers;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Skill Config", menuName = "Skills/Skill")]
    public class SkillControlSettingsSO : ScriptableObjectID
    {
        [Space]
        public float GrowTime;
        [Range(0.01f, 10)] public float StartRad;
        [Range(0.01f, 10)] public float EndRad;
        [Space] public float Cooldown;
        [Range(0, 100)] public int Cost;
        [Space] public BaseStatTriggerConfig[] Triggers;
        [Space] public BaseSkill Prefab;
        public EffectsCollection Effects;
    }
}