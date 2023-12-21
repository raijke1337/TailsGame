using Arcatech.Managers;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using UnityEngine;

namespace Arcatech.Items
{
    public class RangedWeapon : BaseWeapon
    {
        [SerializeField, Range(0, 5), Tooltip("time to reload")] protected float _reload = 2f;
        [SerializeField, Range(0, 1), Tooltip("spread of shots")] protected float _spreadMax = 0.1f;
        protected int shotsToDo = 1;

        [SerializeField] private ProjectileConfiguration _projectile;      

        public SimpleEventsHandler<ProjectileComponent> PlacedProjectileEvent;
        protected void CreateProjectile()
        {
            WeaponUses.ChangeCurrent(-1);
            var p = Instantiate(_projectile.ProjectilePrefab);
            p.Setup(_projectile.Settings, Owner);

            PlacedProjectileEvent?.Invoke(p);

            ProjectileSubs(p, true);
        }



        protected void ProjectileSubs(ProjectileComponent p, bool isSub)
        {
            if (isSub)
            {
                p.ProjectileEnteredTriggerEvent += CheckForProjectileHit;
                p.ProjectileExpiredEvent += (t) => ProjectileSubs(t, false);
            }
            else
            {
                p.ProjectileEnteredTriggerEvent -= CheckForProjectileHit;
            }

        }

        private void CheckForProjectileHit(Collider c, ProjectileComponent p)
        { 
            if (c.TryGetComponent(out BaseUnit u) && u.Side != Owner.Side)
            {
                TriggerActivationCallback(u);
            }
        }

        protected virtual void Start()
        {
            if (_projectile == null) Debug.LogError($"Set projectile for {this}");
        }
        #region weapon
        public override bool UseWeapon()
        {
            bool ok = base.UseWeapon();

            if (IsBusy)
            {
                return false;
            }

            if (ok)
            {
                IsBusy = true;
                StartCoroutine(ShootingCoroutine(shotsToDo));
            }
            return ok;
        }

        protected virtual IEnumerator ShootingCoroutine(int shots)
        {
            CreateProjectile();
            IsBusy = false;
            CheckReload();
            yield return null;
        }
        protected virtual IEnumerator ReloadCoroutine()
        {
            IsBusy = true;
            yield return new WaitForSeconds(_reload);
            WeaponUses.ChangeCurrent(WeaponUses.GetMax);
            IsBusy = false;
        }


        protected override void FinishWeaponConfig()
        {
        }


        protected virtual void CheckReload()
        {
            if (WeaponUses.GetCurrent <= 0) StartCoroutine(ReloadCoroutine());
        }
        #endregion



    }

}