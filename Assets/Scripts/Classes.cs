using Arcatech.Items;
using Arcatech.Scenes;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
        public static class StateMachineData
        {
            public const string c_MethodPrefix = "Fsm_";
        }

        #endregion
        #region tools

    }
    [Serializable]
    public class Timer
    {
        public delegate void TimerEvents<T>(T arg) where T : Timer;
        public event TimerEvents<Timer> TimeUp;
        public float GetInitial { get; }
        public float GetRemaining { get; private set; }
        public bool GetExpired { get => _expired; }
        private bool _expired;
        public Timer(float t)
        {
            GetInitial = t;
            GetRemaining = t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltatime"></param>
        /// <returns> current remaining time</returns>
        public float TimerTick(float deltatime)
        {
            if (GetRemaining <= 0 && !_expired)
            {
                _expired = true;
                TimeUp?.Invoke(this);
            }
            else if (GetRemaining > 0)
            {
                GetRemaining -= deltatime;
            }
            return GetRemaining;
        }
        public void ResetTimer()
        {
            _expired = false;
            GetRemaining = GetInitial;
        }
    }

    #endregion
    #region stats
    [Serializable]
    public class StatValueContainer
    {
        [SerializeField] private float _start;
        [SerializeField] private float _max;
        [SerializeField] private float _min;



        private float _calcStart;
        private float _calcMax;
        private float _calcMin;
        private float _current;
        private float _last;
        private List<StatValueModifier> _mods;

        /// <summary>
        /// current, previous
        /// </summary>
        public SimpleEventsHandler<float, float> ValueChangedEvent;

        public float GetCurrent { get => _current; }
        public float GetMax { get => _calcMax; }
        public float GetMin { get => _calcMin; }
        public float GetStart { get => _calcStart; }
        public float GetLast { get => _last; }
        public IEnumerable<StatValueModifier> GetMods { get => _mods; }

        /// <summary>
        /// adds the value, clamped by min and max
        /// </summary>
        /// <param name="value">use negative for decrease</param>
        public void ChangeCurrent(float value)
        {
            _last = _current;
            _current = Mathf.Clamp(_current + value, _min, _max);
            ValueChangedEvent?.Invoke(_current, _last);
        }

        public void Setup()
        {
            _current = _start;
            // this is for lazy people
            // case: only start value is set
            if (_max == 0) _max = _start;
            _mods = new List<StatValueModifier>();
            RefreshValues();
        }
        public void AddMod(StatValueModifier mod)
        {
            _mods.Add(mod);
        }
        public void RemoveMod(StatValueModifier mod)
        {
            _mods.Remove(mod);
        }
        public void RemoveMod(string modID)
        {
            var f = _mods.First(t => t.ID == modID);
            if (f == null) return;
            _mods.Remove(f);
        }
        private void RefreshValues()
        {
            _calcMin = _min + _mods.Sum(t => t.MinChange);
            if (_calcMin <= 0f)
            {
                ValueChangedEvent?.Invoke(0f, _min);
            }
            _calcMax = _max + _mods.Sum(t => t.MaxChange);
            _calcStart = _start + _mods.Sum(t => t.StartChange);
        }

        public override string ToString()
        {
            return ($"{Mathf.RoundToInt(GetCurrent)} / {Mathf.RoundToInt(GetMax)}");
        }


        public StatValueContainer(StatValueContainer preset)
        {
            _start = preset._start;
            _max = preset._max;
            _min = preset._min;
            Setup();
        }
    }

    public class StatValueModifier : IHasID
    {
        public string ID = "Not set";
        public float MaxChange;
        public float MinChange;
        public float StartChange;

        public StatValueModifier(float max, float min, float st)
        {
            MaxChange = max; MinChange = min; StartChange = st;
        }
        public string GetID => ID;
    }
    #endregion


    #region items
    [Serializable]
    public class UnitInventoryItemConfigsContainer
    {
        public string ID;


        [SerializeField] public List<Equip> Equipment;
        [SerializeField, Space] public List<Item> Inventory;
        [SerializeField, Space] public List<Item> Drops;
        public UnitInventoryItemConfigsContainer(UnitItemsSO cfg)
        {
            Equipment = new List<Equip>(cfg.Equipment);
            Inventory = new List<Item>();
            Drops = new List<Item>();

            foreach (Item i in cfg.Inventory)
            {
                if (i is Equip e)
                {
                    Inventory.Add(e);
                }
                else
                {
                    Inventory.Add(i);
                }
            }

            foreach (Item i in cfg.Drops)
            {
                if (i is Equip e)
                {
                    Drops.Add(e);
                }
                else
                {
                    Drops.Add(i);
                }
            }
            ID = cfg.ID;
        }
        public UnitInventoryItemConfigsContainer()
        {

        }
    }

    [Serializable]
    public class RangedWeaponConfig
    {

        public ProjectileConfiguration Projectile;

        [Space,SerializeField, Range(1, 20), Tooltip("How projectiles will be spawned until reload is started")] public int Ammo;
        [SerializeField, Range(1, 12), Tooltip("How many projectiles are created per each use")] public int ShotsPerUse;
        [SerializeField, Range(0, 5), Tooltip("Time in reload")] public float Reload;
        [SerializeField, Range(0, 1), Tooltip("Spread of shots (inaccuaracy)")] public float Spread;
    }

    #endregion

}