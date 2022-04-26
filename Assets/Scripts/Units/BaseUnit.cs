
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ControlInputsBase))]
    public abstract class BaseUnit : MonoBehaviour
    {
        [SerializeField] protected string StatsID;
    public string GetID => StatsID;

        protected Animator _animator;
        protected Rigidbody _rigidbody;
        protected BaseStatsController _baseStats;
        protected ControlInputsBase _controller;


        public Side Side;
        public Transform SkillsPosition;

        public T GetInputs<T>() where T : ControlInputsBase => _controller as T;
        [Inject] protected StatsUpdatesHandler _handler;



        public string GetFullName() => _baseStats.GetDisplayName;
        public IReadOnlyDictionary<StatType, StatValueContainer> GetStats() => _baseStats.GetBaseStats;


        public event SimpleEventsHandler<BaseUnit> BaseUnitDiedEvent;
        public event BaseUnitWithIDEvent SkillRequestSuccessEvent;
        protected void SkillRequestCallBack(string id, BaseUnit unit) => SkillRequestSuccessEvent?.Invoke(id, unit);

        private Camera _faceCam;
        public void ToggleCamera(bool value) { _faceCam.enabled = value; }

        protected virtual void Awake()
        {
            _baseStats = new BaseStatsController(StatsID);
        }

        protected virtual void OnEnable()
        {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<ControlInputsBase>();
        _controller.InitControllers(_baseStats);
        _controller.BindControllers(true);
        
        _faceCam = GetComponentsInChildren<Camera>().First(t => t.CompareTag("FaceCamera"));
        _faceCam.enabled = false;
        }

        protected virtual void Start()
        {
            UnitBinds(true);
        }

        protected virtual void OnDisable()
        {
            UnitBinds(false);
        _controller.BindControllers(false);
    }
        protected virtual void FixedUpdate()
        {
            if (_controller.IsControlsBusy) return;
            AnimateMovement();
        }

        public void TriggerTogglingEvent_UE(float value)
        {    // 1 on start 0 on end

            bool result = value > 0;
            _controller.GetWeaponController.ToggleTriggersOnMelee(result);
        }


        // die here
        protected virtual void HealthChangedEvent(float value, float prevValue)
        {
            if (value <= 0f)
            {
                _animator.SetTrigger("Death");
                BaseUnitDiedEvent?.Invoke(this);
                StartCoroutine(RemainsDisappearCoroutine());
            }
        }
        //sets animator values 
        protected virtual void AnimateMovement()
        {
            ref var movement = ref _controller.MoveDirection;
            if (movement.x == 0f && movement.z == 0f)
            {
                _animator.SetBool("Moving", false);
                _animator.SetFloat("ForwardMove", 0f);
                _animator.SetFloat("SideMove", 0f);
            }
            else
            {
                _animator.SetBool("Moving", true);
                CalcAnimVector(movement);
            }
        }
        //  calculates the vector which is used to set values in animator
        protected void CalcAnimVector(Vector3 vector)
        {
            var playerFwd = transform.forward;
            var playerRght = transform.right;
            vector.y = 0;
            vector.Normalize();
            // Dot product of two vectors determines how much they are pointing in the same direction.
            // If the vectors are normalized (transform.forward and right are)
            // then the value will be between -1 and +1.
            var x = Vector3.Dot(playerRght, vector);
            var z = Vector3.Dot(playerFwd, vector);

            _animator.SetFloat("ForwardMove", z);
            _animator.SetFloat("SideMove", x);
        }


        protected virtual void AnimateCombatActivity(CombatActionType type)
        {
            switch (type)
            {
                case CombatActionType.Melee:
                    _animator.SetTrigger("MeleeAttack");
                    break;
                case CombatActionType.Ranged:
                    _animator.SetTrigger("RangedAttack");
                    break;
                case CombatActionType.Dodge:
                    _animator.SetTrigger("Dodge");
                    DodgeAction();
                    break;
                case CombatActionType.MeleeSpecialQ:
                    _animator.SetTrigger("QSpecial");
                SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(CombatActionType.MeleeSpecialQ), this);
                break;
                case CombatActionType.RangedSpecialE:
                    _animator.SetTrigger("ESpecial");
                SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(CombatActionType.RangedSpecialE), this);
                break;
                case CombatActionType.ShieldSpecialR:
                    _animator.SetTrigger("RSpecial");
                SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(CombatActionType.ShieldSpecialR), this);
                break;
            }
        }
        protected virtual void DodgeAction() { }

        protected virtual void UnitBinds(bool isEnable)
        {
            if (isEnable)
            {
                _handler.RegisterUnitForStatUpdates(_baseStats);
                GetStats()[StatType.Health].ValueChangedEvent += HealthChangedEvent;
                _controller.CombatActionSuccessEvent += (t) => AnimateCombatActivity(t);
            _controller.StaggerHappened += AnimateStagger;
            }
            else
            {
                GetStats()[StatType.Health].ValueChangedEvent -= HealthChangedEvent;
                _controller.CombatActionSuccessEvent -= (t) => AnimateCombatActivity(t);
                _handler.RegisterUnitForStatUpdates(_baseStats, false);
            _controller.StaggerHappened += AnimateStagger;
        }
        }
    protected void AnimateStagger()
    {
        _animator.SetTrigger("TakeDamage");
    }

        protected virtual IEnumerator RemainsDisappearCoroutine()
        {
            UnitBinds(false);
            float time = 0f;
            while (time < Constants.Combat.c_RemainsDisappearTimer)
            {
                time += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
            yield return null;
        }

        public abstract void ApplyEffect(TriggeredEffect eff);


        protected void OnValidate()
        {
            if (SkillsPosition == null) SkillsPosition = GetComponentsInChildren<Transform>().First(t => t.name.Contains("Hips"));
        }
    }
