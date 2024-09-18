using System;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Shield Item", menuName = "Items/Shield")]
    public class ShieldSO : EquipSO
    {
        public SerializedShieldAbsorbStrategy absorbStrategy;
    }


}