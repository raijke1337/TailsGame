using Arcatech.EventBus;
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
        [SerializeField] protected bool _showDebugs = false;
        [Header("Entity settings")]
        [SerializeField] protected Side _unitSide;
        [HideInInspector] public Side Side => _unitSide;

        [Space, SerializeField] protected UnitStatsController _stats;
        [SerializeField] protected BaseStatsConfig defaultStats;

        [Space, Header("Actions"), SerializeField] protected SerializedUnitAction ActionOnDamage;
        [SerializeField] protected SerializedUnitAction ActionOnDeath;

        [SerializeField,Self] protected Animator _animator;
        public string GetUnitName { get => defaultStats.DisplayName; }

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

            _stats.StatsUpdatedEvent += RaiseStatChangeEvent;
        }

        public virtual void DisableUnit()
        {
            if (_showDebugs) Debug.Log($"Stopping unit {this}");
            StopAllCoroutines();
        }


        public virtual void RunUpdate(float delta)
        {

            _stats.ControllerUpdate(delta);
        }
        public virtual void RunFixedUpdate(float delta)
        {
            _stats.FixedControllerUpdate(delta);
        }

        #endregion
        #region stats

        public virtual void ApplyEffect(StatsEffect eff)
        {
            _stats.ApplyEffect(eff);
        }
        public event UnityAction<BaseEntity> BaseUnitDiedEvent = delegate { };

        protected virtual void RaiseStatChangeEvent(StatChangedEvent ev)
        {
            switch (ev.StatType)
            {
                case BaseStatType.Health:
                    if (ev.Container.GetCurrent < ev.Container.CachedValue)
                    {
                        EventBus<DrawDamageEvent>.Raise(new DrawDamageEvent(this, ev.Container.GetCurrent - ev.Container.CachedValue));
                        HandleDamage(ev.Container.GetCurrent - ev.Container.CachedValue);
                    }
                    if (ev.Container.GetCurrent <= 0f)
                    {
                        BaseUnitDiedEvent.Invoke(this);
                        HandleDeath();
                    }
                    break;
                case BaseStatType.Stamina:
                    break;
                case BaseStatType.Energy:
                    break;
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