using UnityEngine;
namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "New RanmgedWeaponConfig", menuName = "Configurations/WeaponsRanged", order = 2)]
    public class RangedWeaponConfig : BaseWeaponConfig
    {
        public string ProjectileID;
    }

}