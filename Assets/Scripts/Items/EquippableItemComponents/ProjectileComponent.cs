using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class ProjectileComponent : MonoBehaviour
    {

        #region managed

        public BaseUnit Owner { get; set; }
        private ProjectileSettingsPackage _data;
        public void Setup(ProjectileSettingsPackage set, BaseUnit owner)
        {
            _data = new ProjectileSettingsPackage() { ProjectilePenetration = set.ProjectilePenetration, ProjectileSpeed = set.ProjectileSpeed, TimeToLive = set.TimeToLive };
            Owner = owner;
        }
        public void UpdateInDelta(float deltaTime)
        {
            transform.position += _data.ProjectileSpeed * deltaTime * transform.forward;
            _data.TimeToLive -= deltaTime;
            if (_data.TimeToLive <= 0)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region controls
        public int Decrement(int times)
        {
            _data.ProjectilePenetration -= times;
            return _data.ProjectilePenetration;
        }

        public void StopProjectile(Transform parent)
        {
            // stuck if hits a static or has no penetration
            _data.ProjectileSpeed = 0;
            transform.SetParent(parent, true);
        }

        #endregion



        protected void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"projectile {this.name} hit {other.name}");
            ProjectileEnteredTriggerEvent?.Invoke(other, this);
        }



        private void OnDestroy()
        {
            ProjectileExpiredEvent?.Invoke(this);
        }

        public event SimpleEventsHandler<Collider, ProjectileComponent> ProjectileEnteredTriggerEvent;

        public event SimpleEventsHandler<ProjectileComponent> ProjectileExpiredEvent;




    }
}
