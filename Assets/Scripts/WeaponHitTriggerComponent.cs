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

public class WeaponHitTriggerComponent : MonoBehaviour
{
    private Collider _collider;

    private Unit _unit;
    private WeaponStats _stats;

    public int ID { get; private set; }

    public void SetUpWeapon(Unit unit, WeaponStats stats)
    {
        _unit = unit;
        _stats = stats;
        ID = stats.ID;
        // model = mesh todo
    }

    public void Toggle()
    {
        _collider.enabled = !_collider.enabled;
    }

    public bool Enable
    {
        get => _collider.enabled;
        set => _collider.enabled = value;
    }

    private void Start()
    {
        if (_collider == null) _collider = GetComponent<Collider>(); 
        _collider.enabled = false;
        _collider.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponent<Unit>();
        if (unit == null) return;
        unit.TakeDamage(_stats.Damage);
        // add effects todo
    }
}  



