using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
using Arcatech.Units.Stats;
using KBCore.Refs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    [RequireComponent(typeof(GroundDetectorPlatformCollider))]
    public abstract class ControlInputsBase : ManagedControllerBase, ITakesTriggers
    {
        protected const float zeroF = 0f;
        public BaseUnit Unit { get; set; }


        private bool _inputsLocked;
        [SerializeField] public bool LockInputs
        {
            get
            {
                return _inputsLocked;
            }
            set
            {
                OnLockInputs(value);
                _inputsLocked = value;
                
            }
        }// todo ?
        protected abstract void OnLockInputs(bool isLock);




        [SerializeField] protected BaseStatsConfig _unitStatsConfig;
        public string GetFullName => _unitStatsConfig.displayName;

        protected List<BaseController> _controllers = new List<BaseController>();
        protected float lastDelta;

        [SerializeField] protected ItemEmpties Empties;
        public ItemEmpties GetEmpties => Empties;

        protected UnitStatsController _statsCtrl;

        protected WeaponController _weaponCtrl;
        protected DodgeController _dodgeCtrl;
        protected SkillsController _skillCtrl;
        public DodgeController GetDodgeController => _dodgeCtrl;
        public UnitStatsController GetStatsController => _statsCtrl;
        public SkillsController GetSkillsController => _skillCtrl;
        public WeaponController GetWeaponController => _weaponCtrl;



        [SerializeField, Self] protected GroundDetectorPlatformCollider _groundedPlatform;
        public GroundDetectorPlatformCollider GetGroundCollider => _groundedPlatform;

        #region movement and jumping
        protected abstract void DoHorizontalMovement(float delta);

        #endregion

        public Vector3 GetMovementVector { get; protected set; } 

        #region controller events

        // shared

        public event SimpleEventsHandler<ProjectileComponent> SpawnProjectileEvent;
        public event EffectsManagerEvent EffectEventRequest;

        // this needs to be updated to get unique triggeredeffects from each use of item
        public event TriggerEvent TriggerEventRequest;
        protected void TriggerEventCallBack(BaseUnit target, BaseUnit source, bool isEnter, TriggeredEffect cfg)
        {
            TriggerEventRequest?.Invoke(target, source, isEnter, cfg);
        }

        protected void EffectEventCallback(EffectRequestPackage pack) => EffectEventRequest?.Invoke(pack);
        protected void ProjectileEventCallBack(ProjectileComponent prefab) => SpawnProjectileEvent?.Invoke(prefab);


        //specific 
        public event SimpleEventsHandler StaggerHappened;
        public event SimpleEventsHandler ShieldBreakHappened;
        public event SimpleEventsHandler<float> DamageHappened;
        public event SimpleEventsHandler ZeroHealthHappened;
        public event SkillRequestedEvent SkillSpawnEvent;

        protected virtual void ShieldBreakEventCallback() => ShieldBreakHappened?.Invoke();

        protected void SkillSpawnEventCallback(SkillProjectileComponent data)
        {
            SkillSpawnEvent?.Invoke(data, Unit, Empties.ItemPositions[data.GetProjectileSettings.SpawnPlace]);
        }

        #endregion


        public StatValueContainer AssessStat(TriggerChangedValue stat) => _statsCtrl.AssessStat(stat);

        #region items
        public void AssignDefaultItems(UnitInventoryComponent items)
        {
            items.InventoryUpdateEvent += CheckEquipsOnInventoryUpdate;
            var skillsItems = new List<EquipmentItem>();

            foreach (var item in items.GetCurrentEquips)
            {
                if (item.Skill != null)
                {
                    skillsItems.Add(item);
                }
                EquipmentItem removed = null;
                switch (item.ItemType)
                {
                    default:
                        Debug.LogError($"assigned {item} on {Unit.name} controller but it is not implemented");
                        break;
                    case (EquipmentType.MeleeWeap):
                        _weaponCtrl.LoadItem(item, out removed);
                        break;
                    case (EquipmentType.RangedWeap):
                        _weaponCtrl.LoadItem(item, out removed);
                        break;
                    case (EquipmentType.Shield):
                        _statsCtrl.LoadItem(item, out removed);
                        break;
                    case (EquipmentType.Booster):
                        _dodgeCtrl.LoadItem(item, out removed);
                        break;
                }
            }

            if (_skillCtrl != null) // null in scenes
            {
                foreach (var item in skillsItems)
                {
                    _skillCtrl.LoadItemSkill(item);
                }
            }
        }

        private void CheckEquipsOnInventoryUpdate(UnitInventoryComponent inve)
        {
            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game) return; // something was picked up and no need to refresh invenotry
            // yikes

            Debug.Log($"Inventory update in {Unit}");

            _weaponCtrl.TryRemoveItem(EquipmentType.MeleeWeap, out _);
            _weaponCtrl.TryRemoveItem(EquipmentType.RangedWeap, out _);
            _dodgeCtrl.TryRemoveItem(EquipmentType.Booster, out _);
            _statsCtrl.TryRemoveItem(EquipmentType.Shield, out _);
            // TODO // 


            foreach (EquipmentItem e in inve.GetCurrentEquips)
            {
                switch (e.ItemType)
                {
                    default:
                        Debug.Log($"No equipment logic for equipement {e}, type {e.ItemType}");
                        break;
                    case (EquipmentType.MeleeWeap):
                        _weaponCtrl.LoadItem(e, out _);
                        break;
                    case (EquipmentType.RangedWeap):
                        _weaponCtrl.LoadItem(e, out _);
                        break;
                    case (EquipmentType.Shield):
                        _statsCtrl.LoadItem(e, out _);
                        break;
                    case (EquipmentType.Booster):
                        _dodgeCtrl.LoadItem(e, out _);
                        break;
                }
            }
        }


        #endregion


        #region ManagedController
        public override void UpdateController(float delta)
        {
            if (LockInputs) return;
            lastDelta = delta;
            foreach (BaseController ctrl in _controllers)
            {
                if (ctrl.IsReady)
                    ctrl.UpdateInDelta(delta);
            }

            if (!_groundedPlatform.IsGrounded) return;
            DoHorizontalMovement(delta);
        }
        public override void StartController()
        {
#if UNITY_EDITOR
            if (GameManager.Instance == null) return;
#endif
            switch (GameManager.Instance.GetCurrentLevelData.LevelType)
            {
                case LevelType.Menu:
                    Initialize(false);
                    break;
                case LevelType.Scene:
                    Initialize(false);
                    break;
                case LevelType.Game:
                    Initialize(true);
                    break;
            }
        }
        public override void StopController()
        {
            foreach (var ctrl in _controllers)
            {
                if (ctrl.IsReady)
                {
                    ctrl.StopComp();

                    ControllerSubs(ctrl, false);
                }
            }
        }

        private void Initialize(bool full)
        {

            _statsCtrl = new UnitStatsController(Empties, Unit);
            _statsCtrl.CurrentStats = new Dictionary<BaseStatType, StatValueContainer>();

            foreach (var k in _unitStatsConfig.Stats.Keys)
            {
                _statsCtrl.CurrentStats[k] = new StatValueContainer(_unitStatsConfig.Stats[k]);
            }

            _dodgeCtrl = new DodgeController(Empties, Unit);
            _weaponCtrl = new WeaponController(Empties, Unit);
            _controllers.Add(_statsCtrl);
            _controllers.Add(_dodgeCtrl);
            _controllers.Add(_weaponCtrl);

            if (full)
            {
                _skillCtrl = new SkillsController(Empties, Unit);
                _controllers.Add(_skillCtrl);
                LockInputs = false;
            }

            foreach (var ctrl in _controllers)
            {
                ctrl.StartComp();
                ControllerSubs(ctrl, true);
            }
        }

        protected virtual void ControllerSubs(BaseController ctrl, bool isStart)
        {
            if (isStart)
            {

                if (ctrl is UnitStatsController st)
                {
                    st.UnitTookDamageEvent += OnDamageTaken;
                    st.ZeroHealthEvent += OnZeroHealth;
                }

                ctrl.BaseControllerEffectEvent += EffectEventCallback;
                ctrl.BaseControllerTriggerEvent += TriggerEventCallBack;
                ctrl.BaseControllerProjectileEvent += ProjectileEventCallBack;

            }
            else
            {
                ctrl.BaseControllerEffectEvent -= EffectEventCallback;
                ctrl.BaseControllerTriggerEvent -= TriggerEventCallBack;
                ctrl.BaseControllerProjectileEvent -= ProjectileEventCallBack;

                if (ctrl is UnitStatsController st)
                {
                    st.UnitTookDamageEvent -= OnDamageTaken;
                    st.ZeroHealthEvent -= OnZeroHealth;
                }
            }
        }
                #endregion




        #region triggers


        public void ApplyEffect(TriggeredEffect eff)
        {
            _statsCtrl.ApplyEffect(eff);
        }

        #endregion


        #region unit actions
        public bool IsInMeleeCombo = false;

        public event UnityAction<UnitActionType> CombatActionAnimationRequest = delegate { };
        private void AnimateACtionCallback(UnitActionType type) => CombatActionAnimationRequest?.Invoke(type);


        public void ToggleBusyControls_AnimationEvent(int state)
        {
            if (DebugMessage)
            {
                Debug.Log($"Animation event busy controls! {state}");
            }
            LockInputs = state != 0;
        }
        protected virtual void RequestCombatAction(UnitActionType type, bool jumpBool = false)
        {
            bool canAct = true;

            if (LockInputs && !IsInMeleeCombo || !_groundedPlatform.IsGrounded)
            {
                return;
            }
            if (DebugMessage)
            {
                Debug.Log($"Do combat action {type}");
            }
            switch (type)
            {
                case UnitActionType.Melee:
                    if (canAct || IsInMeleeCombo)
                    {
                        canAct = _weaponCtrl.OnWeaponUseSuccessCheck(EquipmentType.MeleeWeap);
                        if (canAct) AnimateACtionCallback(type);
                    }
                    break;
                case UnitActionType.Ranged:
                    if (canAct)
                    {
                        canAct = _weaponCtrl.OnWeaponUseSuccessCheck(EquipmentType.RangedWeap);
                        if (canAct) AnimateACtionCallback(type);
                    }
                    break;
                case UnitActionType.Jump:
                    if (canAct)
                    {
                        DoJump(jumpBool);
                        AnimateACtionCallback(UnitActionType.Jump);
                    }
                    break;
                default:
                    if (canAct && _skillCtrl.IsSkillReady(type, out var cost))
                    {
                        if (_statsCtrl.AssessStat(cost.StatType).HasCapacity(cost.InitialValue))
                        {
                            var sk = _skillCtrl.ProduceSkill(type);
                            _statsCtrl.ApplyEffect(cost);

                            SkillSpawnEventCallback(sk);
                            AnimateACtionCallback(type);
                        }
                    }
                    break;
            }
        }

        protected abstract void DoJump(bool jumpBool);

        #endregion



        #region stats events

        protected virtual void OnZeroHealth()
        {
            ZeroHealthHappened?.Invoke();
        }

        protected virtual void OnDamageTaken(float arg)
        {
            DamageHappened?.Invoke(arg);

        }
        #endregion

    }
}