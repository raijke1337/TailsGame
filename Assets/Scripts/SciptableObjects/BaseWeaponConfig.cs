using Arcatech;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseWeaponConfig", menuName = "Configurations/Weapons", order = 1)]
public class BaseWeaponConfig : ScriptableObjectID
{
    public StatValueContainer Charges;
    public List<string> TriggerIDs;

    public float InternalCooldown;

}

