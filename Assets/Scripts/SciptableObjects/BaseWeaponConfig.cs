using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseWeaponConfig", menuName = "Configurations/Weapons", order = 1)]
public class BaseWeaponConfig : ScriptableObjectID
{
    public StatValueContainer Charges;
    public List<string> TriggerIDs;
   //  public string SkillID; removed because this is now stored in ItemContent
    public int ComboValue;
    // how much combo increases per hit
    public float InternalCooldown;

}

