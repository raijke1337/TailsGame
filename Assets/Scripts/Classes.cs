using Arcatech.Items;
using Arcatech.Scenes;
using Arcatech.Triggers;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using UnityEngine;

namespace Arcatech
{

    #region saves


    #endregion

    #region const
    public static class Constants
    {
        public static class Configs
        {
            public const string c_AllConfigsPath = "/Resources/Configurations/";
            public const string c_SavesPath = "/Saves/Save.xml";
            public const string c_LevelsPath = "/Resources/Levels";
        }
        public static class Objects
        {
            public const string c_isoCameraTargetObjectName = "IsoCamTarget";
        }
        public static class Combat
        {
            public const float c_RemainsDisappearTimer = 3f;
            public const float c_StaggeringHitHealthPercent = 0.1f; // 10% max hp
        }
        public static class PrefabsPaths
        {
            public const string c_ItemPrefabsPath = "/Resources/Prefabs/Items/";
            public const string c_SkillPrefabs = "/Resources/Prefabs/Skills/";
            public const string c_InterfacePrefabs = "/Resources/Prefabs/Interface/";
        }
        public static class Texts
        {
            public const string c_TextsPath = "/Resources/Texts/";
            public const string c_WeaponsDesc = "Assets/Resources/Texts/Descriptions/Weapons/";
            public const string c_SkillsDesc = "Assets/Resources/Texts/Descriptions/Skills/";
        }
        public static class StateMachineData
        {
            public const string c_MethodPrefix = "Fsm_";
        }

        #endregion
        #region tools

    }

#endregion
#region items

    [Serializable]
    public class RangedWeaponConfig
    {

        public SerializedProjectileConfiguration Projectile;

        [Space,SerializeField, Range(1, 20), Tooltip("How projectiles will be spawned until reload is started")] public int Ammo;
        [SerializeField, Range(1, 12), Tooltip("How many projectiles are created per each use")] public int ShotsPerUse;
        [SerializeField, Range(0, 5), Tooltip("Time in reload")] public float Reload;
        [SerializeField, Range(0, 1), Tooltip("Spread of shots (inaccuaracy)")] public float Spread;
    }

    #endregion



}