
using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using AYellowpaper.SerializedCollections;
using CartoonFX;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech
{

    public delegate void SimpleEventsHandler();
    public delegate void SimpleEventsHandler<T>(T arg);
    public delegate void SimpleEventsHandler<T1, T2>(T1 arg1, T2 arg2);



    public delegate void WeaponEvents<T>(T arg);
    public delegate void DodgeEvents<T>(T arg);
    public delegate void SkillEvents<T>(T arg);

    public delegate void StateMachineEvent();
    public delegate void StateMachineEvent<T>(T arg);


    public delegate void SkillRequestedEvent(SkillObjectForControls data, BaseUnit source, Transform where);
    public delegate void EffectsManagerEvent(EffectRequestPackage effectRequestPackage);

    public delegate void SimpleTriggerEvent(BaseUnit target,bool isEnter);
    public delegate void TriggerEvent(BaseUnit target, BaseUnit source,bool isEnter,BaseStatTriggerConfig cfg);


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
            _title = c.Title; _texts = c.Texts;
        }

        private string _title;
        public string GetTitle { get => _title; }

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
        public SerializedDictionary<EquipItemType, Transform> ItemPositions;
    }
    #endregion




    #region interfaces

    public interface IHasEffects
    {
        public event EffectsManagerEvent BaseControllerEffectEvent;
    }

    public interface ITakesTriggers
    {
        void PickTriggeredEffectHandler(TriggeredEffect effect);
    }

    public interface IStatsComponentForHandler : IManaged
    {
        bool IsReady { get; }
        event SimpleEventsHandler<bool, IStatsComponentForHandler> ComponentChangedStateToEvent;

       // void Ping();
    }
    public interface IManaged
    {
        void UpdateInDelta(float deltaTime);
        void SetupStatsComponent();
        void StopStatsComponent();
    }

    public interface IHasID
    { string GetID { get; } }
    public interface IHasGameObject
    { public GameObject GetObject(); }
    public interface IHasOwner
    { BaseUnit Owner { get; } }

    public interface IAppliesTriggers : IHasGameObject
    {
        event SimpleTriggerEvent TriggerHitUnitEvent;
    }


    public interface IExpires : IHasGameObject
    {
        event SimpleEventsHandler<IExpires> SkillComponentExpiredEvent;
    }
    //public interface ISkill : IHasID, IAppliesTriggers, IExpires, IHasGameObject
    //{
    //    void OnUse();
    //    void OnUpdate(float delta);
    //}

    public interface INeedsEmpties
    { ItemEmpties Empties { get; } }

    #endregion


}