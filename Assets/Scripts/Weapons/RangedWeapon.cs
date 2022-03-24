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
    [SerializeField, Range(3,20), Tooltip("speed of projectile")] protected float projectileSpeed = 5f;
    [SerializeField, Range(0, 1), Tooltip("spread of shots")] protected float _spreadMax = 0.1f;
    [SerializeField, Range(0, 10), Tooltip("targets a projectile penetrates")] protected int _pen = 2;
    
    
    [SerializeField] protected string _projectileID;
    private GameObject _projectilePrefab;
    private ProjectileConfig _projectileCfg;

    protected int shotsToDo = 1;
    [Inject] TriggersManager _tMan;


    protected virtual void Start()
    {
        _currentCharges = MaxCharges;
        _projectilePrefab = Extensions.GetAssetsFromPath<GameObject>(Constants.c_WeaponPrefabsPath).First(t => t.name == _projectileID);
        _projectileCfg = Extensions.GetAssetsFromPath<ProjectileConfig>(Constants.c_ProjectileConfigsPath).First(t => t.name == _projectileID);
    }

    public override bool UseWeapon()
    {
        if (IsBusy) return false;
        else
        {
            IsBusy = true;
            StartCoroutine(ShootingCoroutine(shotsToDo));
            return true;
        }
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
        pr.transform.forward = _player.transform.forward + new Vector3(0f, Extensions.GetRandomFloat(_spreadMax),0f);
        // a bit of spread

        var comp = pr.GetComponent<ProjectileTrigger>();
        comp.Setup(_projectileCfg, _tMan);

    }
    protected virtual void CheckReload()
    {
        if (_currentCharges == 0) StartCoroutine(ReloadCoroutine());
    }

}

