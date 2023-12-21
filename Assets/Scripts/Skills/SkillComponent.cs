using Arcatech.Effects;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Skills
{
    [RequireComponent(typeof(SphereCollider))]
    public class SkillComponent : MonoBehaviour

    {
        public BaseUnit Owner { get; set; }
        public SkillControlSettingsSO Data { get; set; }

        protected SkillState CurrentState = SkillState.Placer;

        public SimpleEventsHandler<Collider,SkillState> TriggerEnterEvent;
        public SimpleEventsHandler<SkillComponent> SkillDestroyedEvent;

        protected SphereCollider _collider;


        public void AdvanceStage()
        {
            CurrentState += 1;
            _framesAoe = 0;
            _collider.enabled = false;

            _collider.radius = Data.AoERadius;

            _collider.enabled = true; // this should trigger a second collision
            gameObject.name = "Effect" + Data.Description.Title;
        }
        private void OnTriggerEnter(Collider other)
        {
            //_framesAoe = 0;
            TriggerEnterEvent?.Invoke(other, CurrentState);
        }
        
        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            
        }
        private void Start()
        {
            gameObject.name = "Placer" + Data.Description.Title;
            _collider.radius = Data.PlacerRadius;
        }

        private void OnDestroy()
        {
            SkillDestroyedEvent?.Invoke(this);   
        }

        private int _framesAoe = 0;

        private void Update()
        {
            if (CurrentState == SkillState.AoE)
            {
                _framesAoe++;
            }
            if (_framesAoe >= 5)
            {
                Destroy(gameObject);
            }
        }


        #region gizmo
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, (_collider as SphereCollider).radius);
        }

        #endregion


    }







}