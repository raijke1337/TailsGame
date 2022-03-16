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

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    public string ID;
    public WeaponType WeapType;
    public int _charges;

    protected List<BaseStatTriggerConfig> _effects = new List<BaseStatTriggerConfig>();

    protected virtual void OnEnable()
    {
        gameObject.SetActive(true);
    }
    public abstract bool UseWeapon();

    public virtual void AddTriggerData(BaseStatTriggerConfig effect)
    {
        _effects.Add(effect);
    }

    public GameObject GetObject() => gameObject;

}

