using Arcatech.Items;
using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Items
{
    public class BoosterItem : EquipmentItem
    {

        public BoosterItem(Booster cfg, BaseUnit ow) : base(cfg, ow)
        {
            Skill = cfg.Skill;
        }
    }
}