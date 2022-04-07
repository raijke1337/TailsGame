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
public class BaseWeaponController : MonoBehaviour, IStatsComponentForHandler
{
    [SerializeField]
    protected GameObject[] _weaponPrefabs;
    protected SerializableDictionaryBase<WeaponType, IWeapon> _currentWeapons;

    [SerializeField] protected Transform _meleeWeaponEmpty;
    [SerializeField] protected Transform _rangedWeaponEmpty;
    [SerializeField] protected Transform _sheathedWeaponEmpty;

    
    protected WeaponType CurrentWeaponType { get; private set; } = WeaponType.None;

    protected void SwitchWeapon(WeaponType type)
    {
        GameObject weaponOfType = _currentWeapons[type].GetObject();
        SwitchAnimationLayersEvent?.Invoke(type);
        foreach (var other in _currentWeapons.Keys.Where(t => t != type))
        {
            Sheathe(other);
        }
        switch (type)
        {
            case WeaponType.None:
                break;
            case WeaponType.Melee:
                Equip(type);
                break;
            case WeaponType.Ranged:
                Equip(type);
                break;
        }
        CurrentWeaponType = type;
    }

    private void Sheathe(WeaponType type)
    {
        var item = _currentWeapons[type].GetObject();
        item.transform.SetPositionAndRotation(_sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation);
        item.transform.parent = _sheathedWeaponEmpty;
    }
    private void Equip(WeaponType type)
    {
        var item = _currentWeapons[type].GetObject();
        if (type == WeaponType.Melee)
        {
            item.transform.SetPositionAndRotation(_meleeWeaponEmpty.position, _meleeWeaponEmpty.rotation);
            item.transform.parent = _meleeWeaponEmpty;
        }
        if (type == WeaponType.Ranged)
        {
            item.transform.SetPositionAndRotation(_rangedWeaponEmpty.position, _rangedWeaponEmpty.rotation);
            item.transform.parent = _rangedWeaponEmpty;
        }
    }

    public int GetAmmoByType(WeaponType type)
    {
        return _currentWeapons[type].GetAmmo();
    }

    public virtual bool UseWeaponCheck(WeaponType type)
    {
        if (CurrentWeaponType != type) { SwitchWeapon(type); }
        return _currentWeapons[type].UseWeapon();
    }
    public void ToggleTriggersOnMelee(bool isEnable)
    {
        (_currentWeapons[WeaponType.Melee] as MeleeWeapon).ToggleColliders(isEnable);
    }


    // load weapon stats from configs
    // set trigger info for weapon
    [ContextMenu(itemName:"Run setup")]
    public virtual void SetupStatsComponent()
    {
        _currentWeapons = new SerializableDictionaryBase<WeaponType, IWeapon>();

        foreach (var prefab in _weaponPrefabs)
        {
            // todo use a factory so this doesnt have to be a mono

            BaseWeapon item = GetInstantiatedItem(prefab).GetComponent<BaseWeapon>();

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
    }

    private GameObject GetInstantiatedItem(GameObject prefab)
    {
        return Instantiate(prefab, _sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation, _sheathedWeaponEmpty);
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

    public WeaponSwitchEventHandler SwitchAnimationLayersEvent; // also used for layers switch in playerunit


}

