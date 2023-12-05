using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    public abstract class ControlInputsBase : ManagedControllerBase, ITakesTriggers
    {
        protected BaseUnit Unit;

        public abstract ReferenceUnitType GetUnitType();

        [SerializeField] public bool IsInputsLocked; // todo ?


        protected List<BaseController> _controllers = new List<BaseController>();

        public void SetUnit(BaseUnit u) => Unit = u;

        protected float lastDelta;

        [SerializeField] protected ItemEmpties Empties;
        public ItemEmpties GetEmpties => Empties;

        protected BaseStatsController _statsCtrl;
         protected WeaponController _weaponCtrl;
         protected DodgeController _dodgeCtrl;
        protected ShieldController _shieldCtrl;
         protected SkillsController _skillCtrl;
        protected ComboController _comboCtrl;
        protected StunsController _stunsCtrl;

        public DodgeController GetDodgeController => _dodgeCtrl;
        public ShieldController GetShieldController => _shieldCtrl;
        public BaseStatsController GetStatsController => _statsCtrl;
        public SkillsController GetSkillsController => _skillCtrl;
        public WeaponController GetWeaponController => _weaponCtrl;


        public event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;
        public event SimpleEventsHandler StaggerHappened;

        protected void CombatActionSuccessCallback(CombatActionType type) => CombatActionSuccessEvent?.Invoke(type);
        protected void StunEventCallback() => StaggerHappened?.Invoke();
        protected void SkillSpawnEventCallback(SkillObjectForControls data) => SkillSpawnEvent?.Invoke(data,Unit,Empties.SkillsEmpty);

        public event SkillRequestedEvent SkillSpawnEvent;

        #region items
        public void AssignItems(UnitInventoryComponent items)
        {
            items.EquipmentChangedEvent += OnEquipmentChangedEvent;
            var skillsItems = new List<EquipmentItem>();

            foreach (var item in items.GetCurrentEquips)
            {
                if (item.ItemSkillConfig!=null)
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
                //if (removed != null)
                //{
                //    _skillCtrl.LoadItemSkill(removed, false);  // since skill ctrl has a dict skill will simply be repalced
                //}
            }

            if (_skillCtrl!=null) // null in scenes
            {
                foreach (var item in skillsItems)
                {
                    _skillCtrl.LoadItemSkill(item);
                }
            }

        }

        private void OnEquipmentChangedEvent(InventoryItem item, bool isAdded)
        {
            string removed = string.Empty;

            if (item is EquipmentItem eq) // just in case
            {
                if (isAdded)
                {
                    switch (eq.ItemType)
                    {
                        default:
                            Debug.Log($"{this} had a trigger for {item} adding: {isAdded} and nothing happened");
                            break;
                        case (EquipItemType.MeleeWeap):
                            _weaponCtrl.LoadItem(eq, out _);
                            break;
                        case (EquipItemType.RangedWeap):
                            _weaponCtrl.LoadItem(eq, out _);
                            break;
                        case (EquipItemType.Shield):
                            _shieldCtrl.LoadItem(eq, out _);
                            break;
                        case (EquipItemType.Booster):
                            _dodgeCtrl.LoadItem(eq, out _);
                            break;
                    }
                }
                else
                {
                    switch (eq.ItemType)
                    {

                        default:
                            Debug.Log($"{this} had a trigger for {item} adding: {isAdded} and nothing happened");
                            break;
                        case (EquipItemType.MeleeWeap):
                            _weaponCtrl.RemoveItem(eq.ItemType);
                            break;
                        case (EquipItemType.RangedWeap):
                            _weaponCtrl.RemoveItem(eq.ItemType);
                            break;
                        case (EquipItemType.Shield):
                            _shieldCtrl.RemoveItem(eq.ItemType);
                            break;
                        case (EquipItemType.Booster):
                            _dodgeCtrl.RemoveItem(eq.ItemType);
                            break;
                    }
                }
                _skillCtrl.LoadItemSkill(eq);
            }                        
        }


        #endregion


        #region scenes

        public Vector3 InputDirectionOverride = Vector3.zero;
        #endregion

        #region ManagedController
        public override void UpdateController(float delta)
        {
            lastDelta = delta;
            foreach (BaseController ctrl in _controllers)
            {
                if (ctrl.IsReady)
                ctrl.UpdateInDelta(delta);
            }
        }
        public override void StartController()
        {
            switch (GameManager.Instance.GetCurrentLevelData.Type)
            {
                case LevelType.Menu:
                    Initialize(false);
                    break;
                case LevelType.Scene:
                    Initialize(false);
                    MoveDirectionFromInputs = InputDirectionOverride;
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
                ctrl.StopStatsComponent();
                ctrl.UnitRequestsSound -= Ctrl_SoundPlay;
                ctrl.EffectsParticlePlace -= Ctrl_EffectsParticlePlace;
            }
            _stunsCtrl.StunHappenedEvent -= StunEventCallback;
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

                IsInputsLocked = false;
                _stunsCtrl.StunHappenedEvent += StunEventCallback;
            }

            foreach (var ctrl in _controllers)
            {
                //if (ctrl.IsReady) ctrl.Ping();
                ctrl.SetupStatsComponent();
                ctrl.UnitRequestsSound += Ctrl_SoundPlay;
                ctrl.EffectsParticlePlace += Ctrl_EffectsParticlePlace;
            }
        }

        #endregion

        #region effects 
        public event AudioEvents SoundPlay;
        public event SimpleEventsHandler<CFXR_Effect, Transform> ParticlePlace;
        private void Ctrl_SoundPlay(AudioClip c, Vector3 z) => SoundPlay?.Invoke(c, z);

        private void Ctrl_EffectsParticlePlace(CFXR_Effect eff, Transform where) => ParticlePlace(eff, where);
        #endregion

        #region input

        public void ToggleBusyControls_AnimationEvent(int state)
        {
            IsInputsLocked = state != 0;
        }


        public void AddTriggeredEffect(TriggeredEffect eff)
        {
            switch (eff.StatType)
            {
                case TriggerChangedValue.Health:
                    if (!_shieldCtrl.IsReady)
                    {
                        _statsCtrl.AddTriggeredEffect(eff);
                    }
                    else
                    {
                        _statsCtrl.AddTriggeredEffect(_shieldCtrl.ProcessHealthChange(eff));
                    }
                    break;
                case TriggerChangedValue.Shield:
                    _shieldCtrl.AddTriggeredEffect(eff);
                    break;
                case TriggerChangedValue.Combo:
                    _comboCtrl.AddTriggeredEffect(eff);
                    break;
                case TriggerChangedValue.MoveSpeed:
                    _statsCtrl.AddTriggeredEffect(eff);
                    break;
                case TriggerChangedValue.TurnSpeed:
                    _statsCtrl.AddTriggeredEffect(eff);
                    break;
                case TriggerChangedValue.Stagger:
                    _stunsCtrl.AddTriggeredEffect(eff);
                    break;
            }
        }
        #endregion

        #region movement



        public ref Vector3 GetMoveDirection => ref MoveDirectionFromInputs;
        protected Vector3 MoveDirectionFromInputs;

        protected virtual void LerpRotateToTarget(Vector3 looktarget, float delta)
        {

            Vector3 relativePosition = looktarget - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
                delta * _statsCtrl.GetBaseStats[BaseStatType.TurnSpeed].GetCurrent);
        }


        #endregion


        #region dodging

        // OLD //
        // new is using a generic skill // 


        //public void PerformDodging()
        //{
        //    _dodgeCor = StartCoroutine(DodgingMovement());
        //}

        private Coroutine _dodgeCor;
        // stop the dodge like this

        private void OnCollisionEnter(Collision collision)
        {
            if (_dodgeCor != null && !collision.gameObject.CompareTag("Ground"))
            {
                IsInputsLocked = false;
                StopCoroutine(_dodgeCor);
            }
        }

        private IEnumerator DodgingMovement()
        {
            var stats = _dodgeCtrl.GetDodgeStats;
            IsInputsLocked = true;

            Vector3 start = transform.position;
            Vector3 end = start + GetMoveDirection * stats[DodgeStatType.Range].GetCurrent;

            float p = 0f;
            while (p <= 1f)
            {
                p += Time.deltaTime * stats[DodgeStatType.Speed].GetCurrent;
                transform.position = Vector3.Lerp(start, end, p);
                yield return null;
            }
            IsInputsLocked = false;
            yield return null;
        }


        #endregion

    }

}