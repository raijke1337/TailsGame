
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Dodge Skill Config", menuName = "Skills/Dodge Skill")]
    public class DodgeSkillConfigurationDictionarySO : SkillControlSettingsSO
    {
        [Space] public SerializedDictionary<DodgeStatType, StatValueContainer> DodgeSkillStats;
    }

}