using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WeaponController : BaseController, IGivesSkills, IHasOwner
{

    #region inventoryManager

    public Dictionary<EquipItemType, IWeapon> CurrentWeapons = new Dictionary<EquipItemType, IWeapon>();
    public ItemEmpties Empties { get; }
    public WeaponController(ItemEmpties ie) => Empties = ie;


    public void LoadItem(IEquippable item)
    {
        if (!(item is IWeapon)) return;
        else
        {
            var weap = GameObject.Instantiate(item.GetEquipmentBase(), Empties.SheathedWeaponEmpty.position, Empties.SheathedWeaponEmpty.rotation, Empties.SheathedWeaponEmpty);
            var w = weap as BaseWeapon;
            CurrentWeapons[item.GetContents.ItemType] = w;

            BaseWeaponConfig config = Extensions.GetConfigByID<BaseWeaponConfig>(w.GetID);

            if (config == null)
            {
                IsReady = false;
                throw new Exception($"Failed to config weapon {w.GetContents.ID} on {Owner}");
            }

            w.SetUpWeapon(config);
            w.Owner = Owner;


            IsReady = true;
            // here we replace the actual prefab that is stored in the ItemContent with our cloned one
        }
    }

    #endregion

    public WeaponSwitchEventHandler SwitchAnimationLayersEvent; // also used for layers switch in playerunit
    public BaseUnit Owner { get; set; }

    public EquipItemType CurrentWeaponType { get; private set; } = EquipItemType.None;

    //public bool IsReady { get; private set; } = false;

    public void SwitchModels(EquipItemType type) => SwitchWeapon(type);
    protected virtual void SwitchWeapon(EquipItemType type)
    {
        GameObject weaponOfType = CurrentWeapons[type].GetObject();
        SwitchAnimationLayersEvent?.Invoke(type);
        foreach (var other in CurrentWeapons.Keys.Where(t => t != type))
        {
            Sheathe(other);
        }
        switch (type)
        {
            case EquipItemType.None:
                break;
            case EquipItemType.MeleeWeap:
                Equip(type);
                break;
            case EquipItemType.RangedWeap:
                Equip(type);
                break;
        }
        CurrentWeaponType = type;
        
    }

    protected void Sheathe(EquipItemType type)
    {
        var item = CurrentWeapons[type].GetObject();
        item.transform.SetPositionAndRotation(Empties.SheathedWeaponEmpty.position, Empties.SheathedWeaponEmpty.rotation);
        item.transform.parent = Empties.SheathedWeaponEmpty;
    }
    protected bool Equip(EquipItemType type)
    {
        if (!CurrentWeapons.ContainsKey(type)) return false;

        var item = CurrentWeapons[type].GetObject();
        if (item == null) return false;
        if (type == EquipItemType.MeleeWeap)
        {
            item.transform.SetPositionAndRotation(Empties.MeleeWeaponEmpty.position, Empties.MeleeWeaponEmpty.rotation);
            item.transform.parent = Empties.MeleeWeaponEmpty;
        }
        if (type == EquipItemType.RangedWeap)
        {
            item.transform.SetPositionAndRotation(Empties.RangedWeaponEmpty.position, Empties.RangedWeaponEmpty.rotation);
            item.transform.parent = Empties.RangedWeaponEmpty;
        }
        return true;
    }

    public int GetAmmoByType(EquipItemType type)
    {
        if (!CurrentWeapons.ContainsKey(type)) return 0;
        else return CurrentWeapons[type].GetAmmo;
    }

    public virtual bool UseWeaponCheck(EquipItemType type,out string result)
    {
        if (!CurrentWeapons.ContainsKey(type))
        {
            result = ($"{Owner.GetFullName} used {type} but has no weapon of this type available;");
            return false;
        }

        if (CurrentWeaponType != type) { SwitchWeapon(type); }
        bool isOk = CurrentWeapons[type].UseWeapon(out var s);

        if (isOk) result = ($"{Owner.GetFullName} used {CurrentWeapons[type]}");
        else result = ($"{Owner.GetFullName} used {CurrentWeapons[type]} but failed: {s}");
        return isOk;

    }
    public void ToggleTriggersOnMelee(bool isEnable)
    {
        if (CurrentWeapons.ContainsKey(EquipItemType.MeleeWeap))
        (CurrentWeapons[EquipItemType.MeleeWeap] as MeleeWeapon).ToggleColliders(isEnable);
    }


    public override void SetupStatsComponent()
    {

    }


    public override void UpdateInDelta(float deltaTime)
    {
        foreach (var w in CurrentWeapons.Values)
        {
            w.UpdateInDelta(deltaTime);
        }
    }

    public IEnumerable<string> GetSkillStrings()
    {
        var list = new List<string>();
        foreach (IWeapon w in CurrentWeapons.Values)
        {
            list.Add(w.GetContents.SkillString);
        }
        return list;
    }
}

