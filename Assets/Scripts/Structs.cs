
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace Arcatech
{

    public delegate void SimpleEventsHandler();
    public delegate void SimpleEventsHandler<T>(T arg);
    public delegate void SimpleEventsHandler<T1, T2>(T1 arg1, T2 arg2);

    public delegate void TriggerEventApplication(string ID, BaseUnit target, BaseUnit source);
    public delegate void SkillRequestedEvent(string ID, BaseUnit source, Transform where);

    public delegate void WeaponEvents<T>(T arg);
    public delegate void DodgeEvents<T>(T arg);
    public delegate void SkillEvents<T>(T arg);

    public delegate void StateMachineEvent();
    public delegate void StateMachineEvent<T>(T arg);




    #region structs 

    [Serializable] public class ItemsStringsSave
    {
        public List<string> Equips;
        public List<string> Inventory;
        public ItemsStringsSave()
        {
            Equips = new List<string>();
            Inventory = new List<string>();
        }
    }


    [Serializable]
    public struct SkillData
    {
        public Sprite Icon;
        public float Recharge;
        public float FinalArea;
        public float StartArea;
        public float PersistTime;
        public float SkillCost;
        public string[] TriggerIDs;
        public AudioComponentBase AudioData;


        public SkillData(SkillData refs)
        {
            Icon = refs.Icon; Recharge = refs.Recharge; FinalArea = refs.FinalArea; StartArea = refs.StartArea; PersistTime = refs.PersistTime; SkillCost = refs.SkillCost; //TargetType = refs.TargetType;
            TriggerIDs = refs.TriggerIDs; AudioData = refs.AudioData;
        }
    }
    [Serializable]
    public struct EnemyStats
    {
        public float AttackRange;

        public float LookSpereCastRadius;
        public float LookSpereCastRange;

        public ReferenceUnitType EnemyType;
        public EnemyStats(EnemyStatsConfig cfg)
        {
            LookSpereCastRadius = cfg.lookSphereRad;
            LookSpereCastRange = cfg.lookRange;
            AttackRange = cfg.atkRange;
            EnemyType = cfg.Type;
        }
    }

    [Serializable]
    public struct TextContainer
    {
        public TextContainer(TextContainerSO c)
        {
            ID = c.ID; _title = c.Title; _texts = c.Texts;
        }

        private string _title;
        public string GetTitle { get => _title; }

        public string ID { get; }
        string[] _texts;

        public string GetFormattedText
        {
            get
            {
                string result = string.Empty;

                foreach (string rec in _texts)
                {
                    result += $"{rec} \n";
                }

                return result;
            }
        }

    }
    [Serializable]
    public class ItemEmpties
    {
        public Transform MeleeWeaponEmpty;
        public Transform RangedWeaponEmpty;
        public Transform SheathedWeaponEmpty;
        public Transform SkillsEmpty;
        public Transform OthersEmpty;
        public void ParentAndAdjust(Transform item)
        {
            // nyi
        }

    }

    #endregion




    #region interfaces

    public interface ITakesTriggers
    {
        void AddTriggeredEffect(TriggeredEffect effect);
    }

    public interface IStatsComponentForHandler
    {
        bool IsReady { get; }
        event SimpleEventsHandler<bool, IStatsComponentForHandler> ComponentChangedStateToEvent;
        void UpdateInDelta(float deltaTime);
        void SetupStatsComponent();
        void StopStatsComponent();
       // void Ping();
    }

    public interface IHasID
    { string GetID { get; } }
    public interface IHasGameObject
    { public GameObject GetObject(); }
    public interface IHasOwner
    { BaseUnit Owner { get; } }

    public interface IAppliesTriggers : IHasGameObject
    {
        event TriggerEventApplication TriggerApplicationRequestEvent;
    }

    #region sound

    public delegate void AudioEvents(AudioClip c, Vector3 position);
    public interface IProducesSounds
    {
        public event AudioEvents SoundPlayEvent;

    }

    #endregion
    public interface IExpires : IHasGameObject
    {
        event SimpleEventsHandler<IExpires> HasExpiredEvent;
    }
    public interface ISkill : IHasID, IAppliesTriggers, IHasOwner, IExpires, IHasGameObject
    {
        void OnUse();
        void OnUpdate(float delta);
    }
    public interface IProjectile : ISkill
    {
        void SetProjectileData(ProjectileDataConfig cfg);
    }
    public interface INeedsEmpties
    { ItemEmpties Empties { get; } }

    #endregion


}