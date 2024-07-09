using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Arcatech.Units
{



    public abstract class BaseUnit : ValidatedMonoBehaviour
    {
        [Header("Unit settings")]
        public Side Side;       

        [SerializeField] protected RuntimeAnimatorController _baseAnimator;
        [SerializeField,Self] protected Animator _animator;

        public abstract string GetUnitName { get; protected set; }
        public abstract ReferenceUnitType GetUnitType();

        public bool IsUnitAlive { get; protected set; } = true;

        protected Rigidbody _rb;

        #region lockUnit
        private bool _locked = false;
        public bool LockUnit
        {
            get
            {
                return _locked;
            }
            set
            {
                _locked = value;
            }
        }

        #endregion
        #region managed
        
        public virtual void StartControllerUnit() // this is run by unit manager
        {
            _rb = GetComponent<Rigidbody>();
            Debug.Log($"Starting unit {this}"); 
        }

        public virtual void DisableUnit()
        {            
            StopAllCoroutines();
        }


        public abstract void RunUpdate(float delta);
        public abstract void RunFixedUpdate(float delta);

        #endregion



        public void UnitDodge(BoosterSkillInstanceComponent bs)
        {
            Debug.Log($"whoosh!");
        }
    }
}