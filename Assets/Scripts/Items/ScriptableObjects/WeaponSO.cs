using Arcatech.Effects;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon")]
    public class WeaponSO : EquipSO
    {
        [Header("Use settings")]
        public SerializedWeaponUseStrategy WeaponUseStrategy;
        public SerializedStatsEffectConfig Cost;

        public DrawItemsStrategy DrawStrategy;
        public SerializedEffectsCollection Effects;

        public WeaponAnimationsSet WeaponType;

        protected override void OnValidate()
        {
            base.OnValidate();
            Assert.IsNotNull(Cost);
            Assert.IsFalse(WeaponType == WeaponAnimationsSet.None);
        }

    }
}