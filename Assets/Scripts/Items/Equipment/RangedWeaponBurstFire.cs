using System.Collections;
using UnityEngine;

public class RangedWeaponBurstFire : RangedWeapon
{

    [SerializeField, Range(0.001f, 1), Tooltip("time between shots")] protected float _shotsPause = 0.1f;
    [SerializeField, Range(1, 12), Tooltip("Shots per burst")] protected int _shots = 3;

    protected override void Start()
    {
        base.Start();
        shotsToDo = _shots;
    }

    protected override IEnumerator ShootingCoroutine(int shots)
    {
        IsBusy = true;
        for (int done = 0; done < shots; done++)
        {
            CreateProjectile();
            yield return new WaitForSeconds(_shotsPause);
        }
        IsBusy = false;
        CheckReload();
        yield return null;

    }

}

