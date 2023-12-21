using Arcatech.Effects;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Skills
{
    [RequireComponent(typeof(Collider))]
    public class SkillPlacer : MonoBehaviour, IManaged

    #region  base
    {
        public SimpleEventsHandler<Collider> TriggerEnterEvent;

        public Collider Collider { get; protected set; }
        public float TimeToLive { get; set; }


        #region managed
        public virtual void SetupStatsComponent()
        {
            Collider = GetComponent<Collider>();
            Collider.isTrigger = true;
        }

        public void StopStatsComponent()
        {
            
        }

        public void UpdateInDelta(float deltaTime)
        {
            TimeToLive -= deltaTime;
        }
        #endregion



        private void OnTriggerEnter(Collider other)
        {
              TriggerEnterEvent?.Invoke(other);   
        }
        #endregion
        #region editor
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, (Collider as SphereCollider).radius);
        }

        #endregion


    }





}