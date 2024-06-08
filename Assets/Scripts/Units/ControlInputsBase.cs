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
                    MoveDirectionFromInputs = Vector3.zero;
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

        protected BaseStatsController _statsCtrl;
        protected WeaponController _weaponCtrl;
        protected DodgeController _dodgeCtrl; // checks timer, the actual dodge is a skill
        protected ShieldController _shieldCtrl;
        protected SkillsController _skillCtrl;
        protected ComboController _comboCtrl;
        protected StunsController _stunsCtrl;

        public DodgeController GetDodgeController => _dodgeCtrl;
        public ShieldController GetShieldController => _shieldCtrl;
        public BaseStatsController GetStatsController => _statsCtrl;
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
                case TriggerChangedValue.Shield:
                    return _shieldCtrl.GetShieldStats[ShieldStatType.Shield];
                case TriggerChangedValue.Combo:
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


        #region scenes

        public Vector3 InputDirectionOverride = Vector3.zero;
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
                    if (this is InputsPlayer p)
                    {
                        MoveDirectionFromInputs = InputDirectionOverride; // placeholder moved into condition because it stopped npcs from working in scenes
                    }
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

            _statsCtrl = new BaseStatsController(Unit);
            _shieldCtrl = new ShieldController(Empties, Unit);
            _dodgeCtrl = new DodgeController(Empties, Unit);
            _weaponCtrl = new WeaponController(Empties, Unit);

            _controllers.Add(_statsCtrl);
            _controllers.Add(_shieldCtrl);
            _controllers.Add(_dodgeCtrl);
            _controllers.Add(_weaponCtrl);

            if (full)
            {
                _comboCtrl = new ComboController(Unit);
                _stunsCtrl = new StunsController(Unit);
                _skillCtrl = new SkillsController(Empties, Unit);

                _controllers.Add(_skillCtrl);
                _controllers.Add(_comboCtrl);
                _controllers.Add(_stunsCtrl);

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
                if (ctrl is ShieldController s)
                {
                    s.ShieldBrokenEvent += ShieldBreakEventCallback;
                }
                if (ctrl is BaseStatsController st)
                {
                    st.UnitTookDamageEvent += OnDamageTaken;
                    st.ZeroHealthEvent += OnZeroHealth;
                }
                if (ctrl is StunsController stun)
                {
                    _stunsCtrl.StunHappenedEvent += StunEventCallback;
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

                if (ctrl is ShieldController s)
                {
                    s.ShieldBrokenEvent -= ShieldBreakEventCallback;
                }
                if (ctrl is BaseStatsController st)
                {
                    st.UnitTookDamageEvent -= OnDamageTaken;
                    st.ZeroHealthEvent -= OnZeroHealth;
                }
                if (ctrl is StunsController stun)
                {
                    _stunsCtrl.StunHappenedEvent -= StunEventCallback;
                }
            }
        }


        #endregion

        #region triggers


        public void ApplyEffectToController(TriggeredEffect eff)
        {
            switch (eff.StatType)
            {
                case TriggerChangedValue.Health:
                    if (!_shieldCtrl.IsReady)
                    {
                        _statsCtrl.ApplyEffectToController(eff);
                    }
                    else
                    {
                        _statsCtrl.ApplyEffectToController(_shieldCtrl.ProcessHealthChange(eff));
                    }
                    break;
                case TriggerChangedValue.Shield:
                    _shieldCtrl.ApplyEffectToController(eff);
                    break;
                case TriggerChangedValue.Combo:
                    _comboCtrl.ApplyEffectToController(eff);
                    break;
                case TriggerChangedValue.MoveSpeed:
                    _statsCtrl.ApplyEffectToController(eff);
                    break;
                case TriggerChangedValue.TurnSpeed:
                    _statsCtrl.ApplyEffectToController(eff);
                    break;
                case TriggerChangedValue.Stagger:
                    _stunsCtrl.ApplyEffectToController(eff);
                    break;
            }
        }

        #endregion


        #region movement



        public ref Vector3 GetMoveDirection => ref MoveDirectionFromInputs;
        protected Vector3 MoveDirectionFromInputs;
        [SerializeField] protected float _minimumAngleChangeToRotate;
        protected virtual void LerpRotateToTarget(Vector3 looktarget, float delta)
        {

            if (MoveDirectionFromInputs != Vector3.zero || Mathf.Abs(Unit.RotationValue) > _minimumAngleChangeToRotate)
            {
                Vector3 relativePosition = looktarget - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
                    delta * _statsCtrl.GetBaseStats[BaseStatType.TurnSpeed].GetCurrent);
            }
        }
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
                canAct = false;
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


        #region dodging

        public void StartDodgeMovement(BoosterSkillInstanceComponent bs)
        {
            if (DebugMessage) Debug.Log($"Start dodge for unit: {Unit}");
            _dodgeCor = StartCoroutine(DodgingMovement(bs));
        }

        private Coroutine _dodgeCor;
        private IEnumerator DodgingMovement(BoosterSkillInstanceComponent bs)
        {
            var stats = bs.GetDodgeSettings;
            Vector3 start = transform.position;
            Vector3 end = start + GetMoveDirection * stats.Range;

            LockInputs = true;

            float p = 0f;
            while (p <= 1f)
            {
                p += Time.deltaTime * stats.Speed;
                transform.position = Vector3.Lerp(start, end, p);
                yield return null;
            }
            LockInputs = false;
            yield return null;
        }
        // stop the dodge like this

        protected void OnCollisionEnter(Collision collision)
        {
            if (_dodgeCor != null && !collision.gameObject.CompareTag("Ground"))
            {
                if (DebugMessage) Debug.Log($"Collided with {collision.gameObject.name} with tag {collision.gameObject.tag}\n {Unit} dodge cancelled.");
                LockInputs = false;
                StopCoroutine(_dodgeCor);
                _dodgeCor = null;
            }
        }

        #endregion
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