using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public class BaseEntity : ValidatedMonoBehaviour
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


        public string GetUnitName { get => defaultStats.DisplayName; }
        [HideInInspector] public Side Side => _unitSide;

        #region managed

        protected override void OnValidate()
        {
            base.OnValidate();
            if (defaultStats == null)
            {
                Debug.LogError($"{this} needs assigned stats!");
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


        public virtual void RunUpdate(float delta)
        {
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
            _stats.FixedControllerUpdate(delta);
        }

        #endregion
        #region stats
        CountDownTimer statsUpdateTimer;

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
            if (_stats.CanApplyEffect(eff, out var curr,shield))
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
        }

        protected virtual void HandleDamage(float value)
        {
            if (_showDebugs) Debug.Log($"{GetUnitName} took dmg {value}");
            if (ActionOnDamage != null)
            {
                ForceUnitAction(ActionOnDamage.ProduceAction(this));
            }
        }

        protected virtual void HandleDeath()
        {
            if (_showDebugs) Debug.Log($"{GetUnitName} died");
            if (ActionOnDeath != null)
            {
                ForceUnitAction(ActionOnDeath.ProduceAction(this));
            }
        }
        protected virtual void HandleStun()
        {
            if (_showDebugs) Debug.Log($"{GetUnitName} got stunned");
            if (ActionOnStun != null)
            {
                ForceUnitAction(ActionOnStun.ProduceAction(this));
            }
        }

        public event UnityAction<BaseEntity> BaseEntityDeathEvent = delegate { };

        #endregion


        public virtual void ForceUnitAction(BaseUnitAction act)
        {
            act.DoAction(this);
        }

        public virtual void ApplyForceResultToUnit(float imp, float time)
        {
            Debug.Log($"Tried to apply impulse {imp} over {time} to {GetUnitName} but it has no movement controller component, using rb impulse");
            Rigidbody rb = GetComponent<Rigidbody>();
            rb?.AddForce(Vector3.forward * imp * 5f,ForceMode.Impulse);
        }
    }
}