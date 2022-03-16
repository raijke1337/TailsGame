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

public class RangedWeapon : BaseWeapon
{
    [SerializeField,Range(0,5)]private float _reload = 3f;
    [SerializeField,Range(0.1f,1)]private float _burstTime = 0.3f;
    [SerializeField,Range(1,12)]private int _burstShots = 3;

    private int _currentCharges;

    private void Start()
    {
        _currentCharges = _charges;
    }

    [SerializeField] GameObject _projectilePrefab;
    public override bool UseWeapon()
    {
        if (_currentCharges == 0f)
        {
            StartCoroutine(ReloadCoroutine());
            return false;
        }

        if (_burstShots == 1) { return true; } 
        else
        {
            StartCoroutine(RangedShots(_currentCharges));
            return true;
        }
    }
    //shots deplete charges
    private IEnumerator RangedShots(int charges)
    {
        CreateProjectile();
        yield return null;
    }
    private IEnumerator ReloadCoroutine()
    {
        var progress = 0f;
        while (progress <= _reload)
        {
            progress += Time.deltaTime;
            yield return null;
        }
        _currentCharges = _charges;
        Debug.Log($"Reloaded {name}");
        yield return null;
    }

    private void CreateProjectile()
    {
        var pr = Instantiate(_projectilePrefab, transform.position, transform.rotation);
        pr.AddComponent<ProjectileComponent>()._effects = _effects;
    }

}

