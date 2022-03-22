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

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    public string ID;
    public WeaponType WeapType;

    public int MaxCharges;
    protected int _currentCharges;
    public int GetAmmo() => _currentCharges;

    public ISkill Skill; //todo related skills

    protected bool IsBusy = false;

    [Inject] protected PlayerUnit _player;

    protected List<WeaponHitTrigger> _triggers;
    // triggers are found separately in melee and ranged

    // get recorded into prefab?
    // todo
    [SerializeField] protected List<string> _effectsIDs;



    private void OnEnable()
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


}

