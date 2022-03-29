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
[RequireComponent(typeof(ZenjectDynamicObjectInjection))]
public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    public string ID;

    private WeaponType WeapType;

    protected int MaxCharges;
    protected int _currentCharges;
    public int GetAmmo() => _currentCharges;

    protected string SkillID;
    public string GetRelatedSkillID() => SkillID;

    protected bool IsBusy = false;

    [Inject] protected PlayerUnit _player;

    protected List<WeaponHitTrigger> _triggers;
    // triggers are found separately in melee and ranged

    protected List<string> _effectsIDs;



    protected virtual void OnEnable()
    {
        _effectsIDs = new List<string>();
    }

    public abstract bool UseWeapon();

    // loaded by weaponcontroller
    public virtual void AddTriggerData(string effectID)
    {
        _effectsIDs.Add(effectID);
    }

    public GameObject GetObject() => gameObject;

    public void SetUpWeapon(BaseWeaponConfig config)
    {
        WeapType = config.WType;
        foreach (string triggerID in config.TriggerIDs)
        {
            AddTriggerData(triggerID);
        }
        MaxCharges = config._charges;
        SkillID = config.SkillID;
    }

}

