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

[Serializable]
public abstract class BaseWeaponController : MonoBehaviour, IStatsComponentForHandler
{
    [SerializeField]
    protected GameObject[] _weaponPrefabs;
    protected SerializableDictionaryBase<WeaponType, IWeapon> _currentWeapons;

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


    #region it works

    // load weapon stats from configs
    // set trigger info for weapon
    [ContextMenu(itemName:"Run seetup")]
    public virtual void Setup()
    {
        _currentWeapons = new SerializableDictionaryBase<WeaponType, IWeapon>();

        foreach (var prefab in _weaponPrefabs)
        {
            // todo use a factory so this doesnt have to be a mono
            var spawn = Instantiate(
                prefab, _sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation, _sheathedWeaponEmpty);
            BaseWeapon item = spawn.GetComponent<BaseWeapon>();

            BaseWeaponConfig config = Extensions.GetAssetsFromPath<BaseWeaponConfig>
                (Constants.c_WeapConfigsPath).First(t => t.ID == item.ID);
            item.WeapType = config.WType;

            if (_currentWeapons.ContainsKey(item.WeapType))
            {
                Debug.LogWarning($"something went wrong with weapons, already has {item.WeapType}");
                return;
            }
            _currentWeapons.Add(item.WeapType, item);

            foreach (string triggerID in config.TriggerIDs)
            {
                item.AddTriggerData(triggerID);
            }                        
            item.MaxCharges = config._charges;
        }
    }

    public void UpdateInDelta(float deltaTime)
    {
        // logic here?
    }

    public List<string> GetSkillIDs()
    {
        var list = new List<string>();
        foreach (var weap in _currentWeapons.Values)
        {
            list.Add(weap.GetRelatedSkillID());
        }
        return list;
    }

    /// first need to instantiate the weapon object and THEN add it to dictionary

#endregion
}

