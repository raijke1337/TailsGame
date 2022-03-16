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
using RotaryHeart.Lib.SerializableDictionary;

public abstract class BaseWeaponController : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] _weaponPrefabs;
    protected Dictionary<WeaponType, IWeapon> _currentWeapons;

    [SerializeField] protected Transform _weaponEmpty;
    [SerializeField] protected Transform _sheathedWeaponEmpty;

    public virtual bool UseWeaponCheck(WeaponType type)
    {
        // ammo checks etc are here
        return _currentWeapons[type].UseWeapon();
    }

    // load weapon stats from configs
    // set trigger info for weapons
    protected virtual void Start()
    {
        _currentWeapons = new Dictionary<WeaponType, IWeapon>();

        foreach (var weap in _weaponPrefabs)
        {
            BaseWeapon item = weap.GetComponent<BaseWeapon>();
            BaseWeaponConfig config = Extensions.GetAssetsFromPath<BaseWeaponConfig>
                (Constants.c_WeapConfigsPath).First(t => t.ID == item.ID);
            item.WeapType = config.WType;

            if (_currentWeapons.ContainsKey(item.WeapType))
            {
                Debug.LogWarning($"something went wrong with {name}'s weapons, already has {item.WeapType}");
                return;
            }

            _currentWeapons.Add(item.WeapType, Instantiate
                (item,_sheathedWeaponEmpty.position,_sheathedWeaponEmpty.rotation,_sheathedWeaponEmpty));

            foreach (string triggerID in config.TriggerIDs)
            {
                var assets = Extensions.GetAssetsFromPath<BaseStatTriggerConfig>
                    (Constants.c_TriggersConfigsPath);
                var data = assets.First(t => t.ID == triggerID);
                item.AddTriggerData(data);
            }                        
            item._charges = config._charges;
        }
    }
    /// first need to instantiate the weapon object and THEN add it to dictionary

}

