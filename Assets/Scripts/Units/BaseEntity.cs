using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
using Arcatech.Units.Stats;
using KBCore.Refs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

namespace Arcatech.Units
{
    public class BaseEntity : ValidatedMonoBehaviour, ITargetable
    {
        protected const float zeroF = 0f;
        [Header("Entity")]
        [SerializeField] protected bool _showDebugs = false;
        [SerializeField] protected Side _unitSide;
        [SerializeField] protected BaseStatsConfig defaultStats;
        [SerializeField] protected float statsUpdateFrequency = 0.1f;
        protected UnitStatsController _stats;
        [Space, SerializeField] protected SerializedUnitAction ActionOnDamage;
        [SerializeField] protected SerializedUnitAction ActionOnDeath;
        [SerializeField] protected SerializedUnitAction ActionOnStun;

        [SerializeField, Self] protected Animator _animator;

        public bool UnitDebug => _showDebugs;

        public string GetName { get => defaultStats.DisplayName; }
        [HideInInspector] public Side Side => _unitSide;

        #region managed

        protected override void OnValidate()
        {
            base.OnValidate();
            Assert.IsFalse(defaultStats==null);
            if (_unitSide == Side.EnemySide)
            {
                Assert.IsTrue(gameObject.layer == LayerMask.NameToLayer("EnemiesLayer"));
            }
        }
        public virtual void StartControllerUnit() // this is run by unit manager
        {
            if (_showDebugs) Debug.Log($"Starting unit {this}");
            _stats = new UnitStatsController(defaultStats.InitialStats, this);
            _stats.StartController();
            statsUpdateTimer = new CountDownTimer(statsUpdateFrequency);
            statsUpdateTimer.Start();
        }

        public virtual void DisableUnit()
        {
            if (_showDebugs) Debug.Log($"Stopping unit {this}");
            StopAllCoroutines();
        }

        /// <summary>
        /// we have action lock - on actions
        /// unitpause - on game pause
        /// unitDead - on death
        /// </summary>

        #region locks
        private bool _paused = false;
        public bool UnitPaused
        {
            get
            {
                return _paused;
            }
            set
            {
                if (_showDebugs) Debug.Log($"Entity paused: {value}");
                OnUnitPause(value);
                _paused = value;
            }
        }

        private bool _dead = false;
        public bool UnitDead
        {
            get { return _dead; }
            set
            {
                if (_showDebugs) Debug.Log($"Entity dead: {value}");
                UnitPaused = value;
                _dead = value;
            }
        }

        #endregion


        public virtual void RunUpdate(float delta)
        {
            if (UnitPaused || UnitDead) return;

            _stats.ControllerUpdate(delta);
            if (statsUpdateTimer!=null)
            {
                statsUpdateTimer?.Tick(delta);
                if (statsUpdateTimer.IsReady)
                {
                    UpdateStats();
                    statsUpdateTimer.Reset();
                    statsUpdateTimer.Start();
                }
            }

        }
        public virtual void RunFixedUpdate(float delta)
        {
            if (UnitPaused) return;

            _stats.FixedControllerUpdate(delta);
        }

        #endregion
        #region stats
        CountDownTimer statsUpdateTimer;
        public event UnityAction<BaseEntity> BaseEntityDeathEvent = delegate { };

        protected virtual void UpdateStats()
        {
            var stats = _stats.GetStatValues;
            foreach (var k in stats.Keys)
            {
                switch (k)
                {
                    case BaseStatType.Health:
                        if (stats[k].GetCurrent == 0) HandleDeath();
                        break;
                    case BaseStatType.Stamina:
                        if (stats[k].GetCurrent == 0) HandleStun();
                        break;
                    case BaseStatType.Energy:
                        break;
                }
            }
        }

        public virtual void ApplyEffect(StatsEffect eff, IEquippable shield = null)
        {
            if (_stats.CanApplyEffect(eff, out var curr, shield))
            {
                switch (eff.StatType)
                {
                    case BaseStatType.Health:
                        if (eff.InitialValue < 0)
                        {
                            EventBus<DrawDamageEvent>.Raise(new DrawDamageEvent(this, Mathf.Abs(eff.InitialValue)));
                            HandleDamage(Mathf.Abs(eff.InitialValue));
                        }
                        if (curr == 0) HandleDeath();
                        break;
                    case BaseStatType.Stamina:
                        if (curr == 0) HandleStun();
                        break;
                    case BaseStatType.Energy:
                        break;
                }
            }
            UpdateStats();

        }

        protected virtual void HandleDamage(float value)
        {
            if (_showDebugs) Debug.Log($"{GetName} took dmg {value}");
            if (ActionOnDamage != null)
            {
                ForceUnitAction(ActionOnDamage.ProduceAction(this, transform));
            }
        }

        protected virtual void HandleDeath()
        {
            
            if (ActionOnDeath != null)
            {
                ForceUnitAction(ActionOnDeath.ProduceAction(this,transform));
            }
            if(TryGetComponent<Collider>(out var c))
            {
                c.enabled = false;
            }
            UnitDead = true;
            BaseEntityDeathEvent.Invoke(this);
        }
        protected virtual void HandleStun()
        {
            if (_showDebugs) Debug.Log($"{GetName} got stunned");
            if (ActionOnStun != null)
            {
                ForceUnitAction(ActionOnStun.ProduceAction(this, transform));
            }
        }
        protected virtual void OnUnitPause(bool isPause)
        {
            Debug.Log($"Entity paused: {isPause} and nothing else happened because this is not overwritten");
        }


        #endregion

        #region actions
        public void ForceUnitAction(BaseUnitAction act)
        {
            if (UnitPaused || act == null) return;
            OnForceAction(act);
        }
        protected virtual void OnForceAction(BaseUnitAction act) { }


        public virtual void ApplyForceResultToUnit(float imp, float time)
        {
            Debug.Log($"Tried to apply impulse {imp} over {time} to {GetName} but it has no movement controller component, using rb impulse");
            Rigidbody rb = GetComponent<Rigidbody>();
            rb?.AddForce(Vector3.forward * imp * 5f,ForceMode.Impulse);
        }
        #endregion


        #region itargetable

        public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetDisplayValues => _stats.GetStatValues;
        #endregion
    }



}