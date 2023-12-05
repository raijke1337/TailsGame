using UnityEngine;
namespace Arcatech.Skills
{
    public class ProjectileSkill : BaseSkill, IProjectile
    {
        public void SetProjectileData(ProjectileDataConfig data) => ProjData = new ProjectileData(data);
        private ProjectileData ProjData;

        private float _exp;
        private int _penetr;

        public string GetID => _skillID;

        public void OnUse()
        {
            transform.position += transform.forward;
            _exp = ProjData.TimeToLive;
            _penetr = ProjData.Penetration;
        }

        public void OnUpdate(float delta)
        {
            if (transform == null) return; // case: gameobject was destroyed by manager
            transform.position += ProjData.Speed * delta * transform.forward;
            _exp -= delta;
            if (_exp <= 0f) CallExpiry();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Trigger enter {this} {other.gameObject.name}");
            if (other == null || Owner == null) return; //something weird may happen here
            base.OnTriggerEnter(other);
            if (_penetr > 0)
            {
                _penetr--;
            }
            if (_penetr == 0) CallExpiry();
        }

        public override void UpdateInDelta(float deltaTime)
        {

        }
    }

}