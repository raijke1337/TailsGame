using Arcatech.Units;
using System.Collections;
using UnityEngine;

namespace Arcatech.Items
{
    public class RangedWeaponItem : WeaponItem
    {
        protected RangedWeaponConfig Config;
        protected int _currentAmmo;
        protected bool _rangedBusy = false;
        
        
        public SimpleEventsHandler<ProjectileComponent> PlacedProjectileEvent;

        public RangedWeaponItem(RangedWeapon cfg, BaseUnit ow) : base(cfg, ow)
        {
            Config = cfg.RangedConfig;
            _currentAmmo = cfg.RangedConfig.Ammo;
        }
        // projectiles are created here
        public override bool TryUseItem()
        {
            if (_rangedBusy || _currentCD > 0) return false;
            if (_currentAmmo <= 0)
            {
                _rangedBusy = true;
                _instantiated.StartCoroutine(DoReload(Config.Reload));
                return false;
            }
            else
            {
                _instantiated.OnItemUse();
                _instantiated.StartCoroutine(DoShooting(Config.ShotsPerUse));
                return true;
            }

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
                OnWeapTriggerEvent(u,true);
            }
        }
        private IEnumerator DoReload(float time)
        {
            yield return new WaitForSeconds(time);
            _currentAmmo = Config.Ammo;
            _rangedBusy = false;
        }
        private IEnumerator DoShooting(int shots)
        {
            int shotsDone = 0;
            _rangedBusy = true;
            while (shots > shotsDone)
            {
                Debug.Log("Pew");
                PlacedProjectileEvent(Config.Projectile.GetProjectile(Owner));
                shotsDone++;
                _currentAmmo--;
                yield return new WaitForSeconds(UseCooldown);
            }
            _rangedBusy=false;
            if (_currentAmmo <= 0) _instantiated.StartCoroutine(DoReload(Config.Reload));
            yield return null;
        }
    }
}