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


    public WeaponType CurrentWeaponType { get; private set; } = WeaponType.None;
    //    visual part.

    protected virtual void EquipItem(WeaponType type)
    {
        GameObject on = _currentWeapons[type].GetObject();
        if (type == WeaponType.Ranged)
        {
            on.transform.SetPositionAndRotation(_rangedWeaponEmpty.position, _rangedWeaponEmpty.rotation);
            on.transform.parent = _rangedWeaponEmpty;
        }
        if (type == WeaponType.Melee)
        {
            on.transform.SetPositionAndRotation(_meleeWeaponEmpty.position, _meleeWeaponEmpty.rotation);
            on.transform.parent = _meleeWeaponEmpty;
        }
        CurrentWeaponType = type;
    }
    protected virtual void UnequipItem(WeaponType type)
    {
        GameObject off = _currentWeapons[CurrentWeaponType].GetObject();
        off.transform.SetPositionAndRotation(_sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation);
        off.transform.parent = _sheathedWeaponEmpty;
    }

    public int GetAmmoByType(WeaponType type)
    {
        return _currentWeapons[type].GetAmmo();
    }

    public virtual bool UseWeaponCheck(WeaponType type)
    {
        // ammo checks etc are here
        return _currentWeapons[type].UseWeapon();
    }
    public void ToggleTriggersOnMelee(bool isEnable)
    {
        (_currentWeapons[WeaponType.Melee] as MeleeWeapon).ToggleColliders(isEnable);
    }


    // load weapon stats from configs
    // set trigger info for weapon
    [ContextMenu(itemName:"Run seetup")]
    public virtual void SetupStatsComponent()
    {
        _currentWeapons = new SerializableDictionaryBase<WeaponType, IWeapon>();

        foreach (var prefab in _weaponPrefabs)
        {
            // todo use a factory so this doesnt have to be a mono

            BaseWeapon item = GetSpawn(prefab).GetComponent<BaseWeapon>();

            BaseWeaponConfig config = Extensions.GetAssetsFromPath<BaseWeaponConfig>
                (Constants.Configs.c_WeapConfigsPath).First(t => t.ID == item.ID);

            item.SetUpWeapon(config);

            if (_currentWeapons.ContainsKey(config.WType))
            {
                Debug.LogWarning($"something went wrong with weapons, already has {config.WType}");
                return;
            }
            _currentWeapons.Add(config.WType, item);
        }


        EquipItem(WeaponType.Melee);
    }

    private GameObject GetSpawn(GameObject prefab)
    {
        var spawn = Instantiate(prefab, _sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation, _sheathedWeaponEmpty);
        return spawn;
    }

    public void UpdateInDelta(float deltaTime)
    {
        // logic here? for cooldowns in weapons if they are not Monos
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


}

