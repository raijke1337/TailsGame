using System;

namespace Arcatech.Units
{
    [Serializable]
    public class EnemyWeaponCtrl : WeaponController
    {
        public EnemyWeaponCtrl(ItemEmpties ie) : base(ie)
        {
        }

        public override void SetupStatsComponent()
        {
            base.SetupStatsComponent();
            if (!Equip(EquipItemType.MeleeWeap)) Equip(EquipItemType.RangedWeap);
        }

    }

}