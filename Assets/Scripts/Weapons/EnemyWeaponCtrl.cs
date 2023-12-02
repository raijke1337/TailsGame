using System;

namespace Arcatech.Units
{
    [Serializable]
    public class EnemyWeaponCtrl : WeaponController
    {
        public EnemyWeaponCtrl(ItemEmpties em, BaseUnit ow) : base(em, ow)
        {

        }

        public override void SetupStatsComponent()
        {
            base.SetupStatsComponent();
            if (!Equip(EquipItemType.MeleeWeap)) Equip(EquipItemType.RangedWeap);
        }

    }

}