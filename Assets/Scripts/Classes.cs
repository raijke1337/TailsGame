using Arcatech.Scenes;
using Arcatech.Triggers;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;

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

    #endregion



}