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


        [Space]public SkillArea AreaOfEffect;
        public SphereSettings AreaSettings;
        [Space]public SkillPlacer Placer;
        public SphereSettings PlacerSettings;
        

        [Space] public BaseStatTriggerConfig[] Triggers;
        [Space] public EffectsCollection Effects;
    }
}