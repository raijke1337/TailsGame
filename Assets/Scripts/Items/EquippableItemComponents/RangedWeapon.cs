using Arcatech.Managers;
using Arcatech.Triggers;
using System.Collections;
using UnityEngine;

namespace Arcatech.Items
{
    public class RangedWeapon : BaseWeapon
    {
        [SerializeField, Range(0, 5), Tooltip("time to reload")] protected float _reload = 2f;
        [SerializeField, Range(0, 1), Tooltip("spread of shots")] protected float _spreadMax = 0.1f;


        [SerializeField] private ProjectileTrigger _projectilePrefab;


        protected int shotsToDo = 1;

        private TriggersProjectilesManager _manager;
        public SimpleEventsHandler<IProjectile> PlacedProjectileEvent;

        protected virtual void Start()
        {
            if (_projectilePrefab == null) Debug.LogError($"Set projectile prefab for {this}");
        }

        public override bool UseWeapon(out string reason)
        {
            bool ok = base.UseWeapon(out string result);
            reason = result;
            // todo wtf?

            if (IsBusy)
            {
                reason = "Weapon is busy";
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
        public override void SetUpWeapon(BaseWeaponConfig config)
        {
            base.SetUpWeapon(config);
            _manager = GameManager.Instance.GetGameControllers.TriggersProjectilesManager;
            _manager.RegisterRangedWeapon(this);
        }
        protected virtual void CreateProjectile()
        {

            WeaponUses.ChangeCurrent(-1);

            var pr = Instantiate(_projectilePrefab);
            pr.Owner = Owner;

            pr.transform.position = transform.position;
            pr.transform.forward = Owner.transform.forward;
            PlacedProjectileEvent?.Invoke(pr);

            pr.SetTriggerIDS(_effectsIDs);
        }
        protected virtual void CheckReload()
        {
            if (WeaponUses.GetCurrent <= 0) StartCoroutine(ReloadCoroutine());
        }

    }

}