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

    [SerializeField] protected Transform _meleeWeaponEmpty;
    [SerializeField] protected Transform _rangedWeaponEmpty;
    [SerializeField] protected Transform _sheathedWeaponEmpty;


    public int GetAmmoByType(WeaponType type)
    {
        return _currentWeapons[type].GetAmmo();
    }

    public virtual bool UseWeaponCheck(WeaponType type)
    {
        // ammo checks etc are here
        return _currentWeapons[type].UseWeapon();
    }

    // load weapon stats from configs
    // set trigger info for weapon
    [ContextMenu(itemName:"Run start")]
    protected virtual void Start()
    {
        _currentWeapons = new Dictionary<WeaponType, IWeapon>();

        foreach (var prefab in _weaponPrefabs)
        {
            var spawn = Instantiate(
                prefab, _sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation, _sheathedWeaponEmpty);
            BaseWeapon item = spawn.GetComponent<BaseWeapon>();

            BaseWeaponConfig config = Extensions.GetAssetsFromPath<BaseWeaponConfig>
                (Constants.c_WeapConfigsPath).First(t => t.ID == item.ID);
            item.WeapType = config.WType;

            if (_currentWeapons.ContainsKey(item.WeapType))
            {
                Debug.LogWarning($"something went wrong with {name}'s weapons, already has {item.WeapType}");
                return;
            }
            _currentWeapons.Add(item.WeapType, item);

            foreach (string triggerID in config.TriggerIDs)
            {
                item.AddTriggerData(triggerID);
            }                        
            item.MaxCharges = config._charges;
            //item.RelatedSkill = 
        }
    }
    /// first need to instantiate the weapon object and THEN add it to dictionary

}

