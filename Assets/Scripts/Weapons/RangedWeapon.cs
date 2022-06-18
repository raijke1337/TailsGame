 using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Zenject;

public class RangedWeapon : BaseWeapon
{
    [SerializeField,Range(0,5), Tooltip("time to reload")]protected float _reload = 2f;
    [SerializeField, Range(0, 1), Tooltip("spread of shots")] protected float _spreadMax = 0.1f;
    
    
    [SerializeField] private ProjectileTrigger _projectilePrefab;
    [SerializeField] private string _projectileID;


    protected int shotsToDo = 1;

    public event SimpleEventsHandler<IProjectile> PlacedProjectileEvent;

    protected virtual void Start()
    {
        _currentCharges = MaxCharges;
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
        _currentCharges = MaxCharges;
        IsBusy = false;
    }

    protected virtual void CreateProjectile()
    {
        _currentCharges--;

        var pr = Instantiate(_projectilePrefab, transform.position, transform.rotation);
        pr.Source = Owner;
        pr.transform.forward = Owner.transform.forward;
        pr.SetTriggerIDS(_effectsIDs);
        pr.GetID = _projectileID;

        PlacedProjectileEvent?.Invoke(pr);
    }
    protected virtual void CheckReload()
    {
        if (_currentCharges == 0) StartCoroutine(ReloadCoroutine());
    }

}

