
using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Dodge Skill Config", menuName = "Skills/Dodge Skill")]
    public class DodgeSkillConfigurationSO : SerializedSkillConfiguration
    {
        // [Space] public SerializedDictionary<DodgeStatType, StatValueContainer> DodgeSkillStats;
       [SerializeField] public DodgeSettingsPackage DodgeSettings;

    }


    [Serializable] public struct DodgeSettingsPackage
    {
        [Range(0,10)] public int Range;
        [Range(0, 10)] public float Speed;

        public DodgeSettingsPackage(DodgeSkillConfigurationSO cfg)
        {
            Range = cfg.DodgeSettings.Range; Speed = cfg.DodgeSettings.Speed;
        }

    }

}