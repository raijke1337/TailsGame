using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    public abstract class ControlInputsBase : ManagedControllerBase, ITakesTriggers
    {
        protected BaseUnit Unit;

        private bool _inputsLocked;
        [SerializeField] public bool LockInputs
        {
            get 
            { 
                return _inputsLocked; 
            }
            set 
            {
                if (_inputsLocked == value) return;

                _inputsLocked = value; 
                if (_inputsLocked)
                {
                    if(DebugMessage) Debug.Log($"Lock inputs in {Unit.GetFullName}");
                    _inputsMovement = Vector3.zero;
                }
                else
                {
                    if (DebugMessage)  Debug.Log($"Unlock inputs in {Unit.GetFullName}");
                }
            }
        }// todo ?


        protected List<BaseController> _controllers = new List<BaseController>();

        public void SetUnit(BaseUnit u) => Unit = u;

        protected float lastDelta;

        [SerializeField] protected ItemEmpties Empties;
        public ItemEmpties GetEmpties => Empties;

        protected UnitStatsController _statsCtrl;
        protected WeaponController _weaponCtrl;
        protected DodgeController _dodgeCtrl; // checks timer, the actual dodge is a skill
        protected EnergyController _shieldCtrl;
        protected SkillsController _skillCtrl;
        protected StaminaController _comboCtrl;
        //protected StunsController _stunsCtrl;
        // depreciated in favor of new mechanics

        public DodgeController GetDodgeController => _dodgeCtrl;
        public EnergyController GetShieldController => _shieldCtrl;
        public UnitStatsController GetStatsController => _statsCtrl;
        public SkillsController GetSkillsController => _skillCtrl;
        public WeaponController GetWeaponController => _weaponCtrl;


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

        protected void StunEventCallback() => StaggerHappened?.Invoke();
        protected virtual void ShieldBreakEventCallback() => ShieldBreakHappened?.Invoke();

        protected void SkillSpawnEventCallback(SkillProjectileComponent data)
        {
            SkillSpawnEvent?.Invoke(data, Unit, Empties.ItemPositions[data.GetProjectileSettings.SpawnPlace]);
        }

        #endregion

        #region ai
        public StatValueContainer AssessStat(TriggerChangedValue stat)
        {
            switch (stat)
            {
                case TriggerChangedValue.Health:
                    return _statsCtrl.GetBaseStats[BaseStatType.Health];
                case TriggerChangedValue.Energy:
                    return _shieldCtrl.GetShieldStats[ShieldStatType.Shield];
                case TriggerChangedValue.Stamina:
                    return _comboCtrl.GetComboContainer;
                default:
                    return null;
            }
        }
        #endregion

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
                        Debug.Log($"assigned {item} on {Unit.name} controller but it is not implemented");
                        break;
                    case (EquipItemType.MeleeWeap):
                        _weaponCtrl.LoadItem(item, out removed);
                        break;
                    case (EquipItemType.RangedWeap):
                        _weaponCtrl.LoadItem(item, out removed);
                        break;
                    case (EquipItemType.Shield):
                        _shieldCtrl.LoadItem(item, out removed);
                        break;
                    case (EquipItemType.Booster):
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

            _weaponCtrl.TryRemoveItem(EquipItemType.MeleeWeap, out _);
            _weaponCtrl.TryRemoveItem(EquipItemType.RangedWeap, out _);
            _dodgeCtrl.TryRemoveItem(EquipItemType.Booster, out _);
            _shieldCtrl.TryRemoveItem(EquipItemType.Shield, out _);
            // TODO // 


            foreach (EquipmentItem e in inve.GetCurrentEquips)
            {
                switch (e.ItemType)
                {
                    default:
                        Debug.Log($"No equipment logic for equipement {e}, type {e.ItemType}");
                        break;
                    case (EquipItemType.MeleeWeap):                        
                        _weaponCtrl.LoadItem(e, out _);
                        break;
                    case (EquipItemType.RangedWeap):
                        _weaponCtrl.LoadItem(e, out _);
                        break;
                    case (EquipItemType.Shield):
                        _shieldCtrl.LoadItem(e, out _);
                        break;
                    case (EquipItemType.Booster):
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
            SetMoveDirection();
            SetAimDirection();
            SetRotationCross();
            SetRotationDot();
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
                    ctrl.StopStatsComponent();

                    ControllerSubs(ctrl, false);
                }
            }
        }

        private void Initialize(bool full)
        {

            _statsCtrl = new UnitStatsController(Unit);
            _shieldCtrl = new EnergyController(Empties, Unit);
            _dodgeCtrl = new DodgeController(Empties, Unit);
            _weaponCtrl = new WeaponController(Empties, Unit);

            _controllers.Add(_statsCtrl);
            _controllers.Add(_shieldCtrl);
            _controllers.Add(_dodgeCtrl);
            _controllers.Add(_weaponCtrl);

            if (full)
            {
                _comboCtrl = new StaminaController(Unit);
                //_stunsCtrl = new StunsController(Unit);
                _skillCtrl = new SkillsController(Empties, Unit);

                _controllers.Add(_skillCtrl);
                _controllers.Add(_comboCtrl);
                //_controllers.Add(_stunsCtrl);

                LockInputs = false;

            }

            foreach (var ctrl in _controllers)
            {
                ctrl.SetupStatsComponent();
                ControllerSubs(ctrl, true);
            }
        }

        protected virtual void ControllerSubs(BaseController ctrl, bool isStart)
        {
            if (isStart)
            {
                if (ctrl is EnergyController s)
                {
                    s.ShieldBrokenEvent += ShieldBreakEventCallback;
                }
                if (ctrl is UnitStatsController st)
                {
                    st.UnitTookDamageEvent += OnDamageTaken;
                    st.ZeroHealthEvent += OnZeroHealth;
                }
                //if (ctrl is StunsController stun)
                //{
                //    _stunsCtrl.StunHappenedEvent += StunEventCallback;
                //}

                ctrl.BaseControllerEffectEvent += EffectEventCallback;
                ctrl.BaseControllerTriggerEvent += TriggerEventCallBack;
                ctrl.BaseControllerProjectileEvent += ProjectileEventCallBack;

            }
            else
            {
                ctrl.BaseControllerEffectEvent -= EffectEventCallback;
                ctrl.BaseControllerTriggerEvent -= TriggerEventCallBack;
                ctrl.BaseControllerProjectileEvent -= ProjectileEventCallBack;

                if (ctrl is EnergyController s)
                {
                    s.ShieldBrokenEvent -= ShieldBreakEventCallback;
                }
                if (ctrl is UnitStatsController st)
                {
                    st.UnitTookDamageEvent -= OnDamageTaken;
                    st.ZeroHealthEvent -= OnZeroHealth;
                }
                //if (ctrl is StunsController stun)
                //{
                //    _stunsCtrl.StunHappenedEvent -= StunEventCallback;
                //}
            }
        }


        #endregion

        #region triggers


        public void ApplyEffect(TriggeredEffect eff)
        {
            switch (eff.StatType)
            {
                case TriggerChangedValue.Health:
                    if (!_shieldCtrl.IsReady)
                    {
                        _statsCtrl.ApplyEffect(eff);
                    }
                    else
                    {
                        _statsCtrl.ApplyEffect(_shieldCtrl.ProcessHealthChange(eff));
                    }
                    break;
                case TriggerChangedValue.Energy:
                    _shieldCtrl.ApplyEffect(eff);
                    break;
                case TriggerChangedValue.Stamina:
                    _comboCtrl.ApplyEffect(eff);
                    break;
                case TriggerChangedValue.MoveSpeed:
                    _statsCtrl.ApplyEffect(eff);
                    break;
                //case TriggerChangedValue.Stagger:
                //    _stunsCtrl.ApplyEffect(eff);
                //    break;
            }
        }

        #endregion

        #region movement

        public ref Vector3 GetMoveDirection => ref _inputsMovement;
        public ref Vector3 GetAimDirection => ref _inputsAiming;
        public ref float GetRotationDot => ref _inputsDot;
        public ref float GetRotationDirection => ref _inputsCross;


        protected Vector3 _inputsMovement;
        protected Vector3 _inputsAiming;
        protected float _inputsDot;
        protected float _inputsCross;

        protected abstract void SetMoveDirection();
        protected abstract void SetAimDirection();
        protected abstract void SetRotationDot();
        protected abstract void SetRotationCross();


        public event SimpleEventsHandler JumpCalledEvent;
        protected virtual void DoJumpAction()
        {
            if (!LockInputs)
            JumpCalledEvent?.Invoke();
        }

        #endregion

        #region combat actions

        public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
        private void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
        public bool IsInMeleeCombo = false;
        public void ToggleBusyControls_AnimationEvent(int state)
        {
            if (DebugMessage)
            {
                Debug.Log($"Animation event busy controls! {state}");
            }
            LockInputs = state != 0;
        }
        protected virtual void DoCombatAction (CombatActionType type)
        {
            bool canAct = true;            

            if (LockInputs && !IsInMeleeCombo)
            {
                return;
            }
            switch (type)
            {
                case CombatActionType.Melee:
                    if (canAct || IsInMeleeCombo)
                    {
                        canAct = _weaponCtrl.OnWeaponUseSuccessCheck(EquipItemType.MeleeWeap);
                        if (canAct) CombatActionSuccessCallback(type);
                    }
                    break;
                case CombatActionType.Ranged:
                    if (canAct)
                    {
                        canAct = _weaponCtrl.OnWeaponUseSuccessCheck(EquipItemType.RangedWeap);
                        if (canAct) CombatActionSuccessCallback(type);
                    }
                    break;
                case CombatActionType.Dodge:
                    if (canAct && _skillCtrl.TryUseSkill(type, _comboCtrl, out var sk))
                    {
                        SkillSpawnEventCallback(sk);
                        CombatActionSuccessCallback(type);
                    }
                    break;
                case CombatActionType.MeleeSpecialQ:
                    if (canAct && _skillCtrl.TryUseSkill(type, _comboCtrl, out sk))
                    {
                        SkillSpawnEventCallback(sk);
                        CombatActionSuccessCallback(type);
                    }
                    break;
                case CombatActionType.RangedSpecialE:
                    if (canAct && _skillCtrl.TryUseSkill(type, _comboCtrl, out sk))
                    {
                        SkillSpawnEventCallback(sk);
                        CombatActionSuccessCallback(type);
                    }
                    break;
                case CombatActionType.ShieldSpecialR:
                    if (canAct && _skillCtrl.TryUseSkill(type, _comboCtrl, out sk))
                    {
                        SkillSpawnEventCallback(sk);
                        CombatActionSuccessCallback(type);
                    }
                    break;
            }
        }


        #endregion


        //#region dodging

        /// <summary>
        /// Moved to UNIT for rigidbody implementation
        /// </summary>

        //public void StartDodgeMovement(BoosterSkillInstanceComponent bs)
        //{
        //    if (DebugMessage) Debug.Log($"Start dodge for unit: {Unit}");
        //    _dodgeCor = StartCoroutine(DodgingMovement(bs));
        //}

        //private Coroutine _dodgeCor;
        //protected Vector3 boostVelocity;
        //private IEnumerator DodgingMovement(BoosterSkillInstanceComponent bs)
        //{
        //    var stats = bs.GetDodgeSettings;
        //    Vector3 start = transform.position;
        //    Vector3 end = start + GetMoveDirection * stats.Range;

        //    LockInputs = true;

        //    float p = 0f;
        //    while (p <= 1f)
        //    {
        //        p += Time.deltaTime * stats.Speed;
        //        transform.position = Vector3.Slerp(start, end,p);
        //        yield return null;
        //    }
        //    LockInputs = false;
        //    yield return null;
        //}
        //// stop the dodge like this

        //protected void OnCollisionEnter(Collision collision)
        //{
        //    if (_dodgeCor != null && !collision.gameObject.CompareTag("Ground"))
        //    {
        //        if (DebugMessage) Debug.Log($"Collided with {collision.gameObject.name} with tag {collision.gameObject.tag}\n {Unit} dodge cancelled.");
        //        LockInputs = false;
        //        StopCoroutine(_dodgeCor);
        //        _dodgeCor = null;
        //    }
        //}

       // #endregion
        #region hp

        protected virtual void OnZeroHealth()
        {
            //Debug.Log($"unit dead seen in baseinputs");
            ZeroHealthHappened?.Invoke();
        }

        protected virtual void OnDamageTaken(float arg)
        {
           // Debug.Log($"hp change seen in baseinputs");
            DamageHappened?.Invoke(arg);

        }
        #endregion

    }

}