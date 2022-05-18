using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WeaponController : MonoBehaviour, IStatsComponentForHandler
{
    [SerializeField]
    protected BaseWeapon[] _weaponPrefabs;
    protected SerializableDictionaryBase<WeaponType, IWeapon> _currentWeapons;

    [SerializeField] protected Transform _meleeWeaponEmpty;
    [SerializeField] protected Transform _rangedWeaponEmpty;
    [SerializeField] protected Transform _sheathedWeaponEmpty;

    public WeaponSwitchEventHandler SwitchAnimationLayersEvent; // also used for layers switch in playerunit
    public event SimpleEventsHandler<float> TargetHitByWeaponEvent; // for comboctrl, used by input
    


    public WeaponType GetCurrentWeaponType => CurrentWeaponType;
    protected WeaponType CurrentWeaponType { get; private set; } = WeaponType.None;

    public void SwitchModels(WeaponType type) => SwitchWeapon(type);
    protected virtual void SwitchWeapon(WeaponType type)
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

    protected void Sheathe(WeaponType type)
    {
        var item = _currentWeapons[type].GetObject();
        item.transform.SetPositionAndRotation(_sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation);
        item.transform.parent = _sheathedWeaponEmpty;
    }
    protected void Equip(WeaponType type)
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

    public int GetAmmoByType(WeaponType type) => _currentWeapons[type].GetAmmo;

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

            var item =  Instantiate(prefab, _sheathedWeaponEmpty.position, _sheathedWeaponEmpty.rotation, _sheathedWeaponEmpty);
            
            BaseWeaponConfig config = Extensions.GetAssetsFromPath<BaseWeaponConfig>
                (Constants.Configs.c_WeapConfigsPath).First(t => t.ID == item.itemID);

            item.SetUpWeapon(config);

            if (_currentWeapons.ContainsKey(config.WType))
            {
                Debug.LogWarning($"something went wrong with weapons, already has {config.WType}");
                return;
            }
            _currentWeapons.Add(config.WType, item);
        }
        foreach (IWeapon weapon in _currentWeapons.Values)
        {
            weapon.TargetHit += (f) => TargetHitByWeaponEvent?.Invoke(f); 
        }
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

