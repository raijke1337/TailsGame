using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class ProjectileComponent : MonoBehaviour
    {

        #region managed

        public bool IsSetup { get; protected set; } = false;
        public BaseUnit Owner { get; set; }
        private ProjectileSettingsPackage _projData;
        public EquipItemType SpawnPlace { get; protected set; }

        public ProjectileSettingsPackage GetProjectileSettings => _projData;

        public virtual void Setup(SerializedProjectileConfiguration set, BaseUnit owner)
        {
            _projData = new ProjectileSettingsPackage(set);
            Owner = owner;
            IsSetup = true;
            SpawnPlace = set.SpawnPlace;
        }
        public void UpdateInDelta(float deltaTime)
        {
            transform.position += _projData.ProjectileSpeed * deltaTime * transform.forward;
            _projData.TimeToLive -= deltaTime;
            if (_projData.TimeToLive <= 0)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region controls
        public int Decrement(int times)
        {
            _projData.ProjectilePenetration -= times;
            return _projData.ProjectilePenetration;
        }

        public void StopProjectile(Transform parent)
        {
            // stuck if hits a static or has no penetration
            _projData.ProjectileSpeed = 0;
            _projData.ProjectilePenetration = 0;
            transform.SetParent(parent, true);
        }

        #endregion



        protected void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"projectile {this.name} hit {other.name}");
            ProjectileEnteredTriggerEvent?.Invoke(other, this);
        }



        public virtual void OnDestroy()
        {
            ProjectileExpiredEvent?.Invoke(this);

        }

        public event SimpleEventsHandler<Collider, ProjectileComponent> ProjectileEnteredTriggerEvent;

        public event SimpleEventsHandler<ProjectileComponent> ProjectileExpiredEvent;




    }
}
