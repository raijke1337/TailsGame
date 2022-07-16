using System.Collections;
using UnityEngine;


public class RangedWeapon : BaseWeapon
{
    [SerializeField,Range(0,5), Tooltip("time to reload")]protected float _reload = 2f;
    [SerializeField, Range(0, 1), Tooltip("spread of shots")] protected float _spreadMax = 0.1f;
    
    
    [SerializeField] private ProjectileTrigger _projectilePrefab;


    protected int shotsToDo = 1;

    public event SimpleEventsHandler<IProjectile> PlacedProjectileEvent;

    protected override void Start()
    {
        base.Start();
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

    protected virtual void CreateProjectile()
    {

        WeaponUses.ChangeCurrent(-1);

        var pr = Instantiate(_projectilePrefab);
        pr.Owner = Owner;

        pr.transform.position = transform.position;
        pr.transform.forward = Owner.transform.forward;


        pr.SetTriggerIDS(_effectsIDs);
        PlacedProjectileEvent?.Invoke(pr);
    }
    protected virtual void CheckReload()
    {
        if (WeaponUses.GetCurrent <= 0) StartCoroutine(ReloadCoroutine());
    }

}

