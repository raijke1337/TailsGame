using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using Arcatech.UI;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;

namespace Arcatech.Skills
{
    public class Skill : ISkill
    {
        #region interface
        public BaseUnit Owner { get ; set; }
        protected SerializedSkill Config { get; }
        public UnitActionType UseActionType => Config.UnitActionType;
        public StatsEffect GetCost => new(Config.CostTrigger);

        #endregion

        protected BaseSkillUsageStrategy Strategy { get; }


        public Skill(SerializedSkill settings, BaseUnit owner, BaseEquippableItemComponent item)
        { 

            Config = settings;
            Owner = owner;
            if (settings == null) return; // placeholder maybe TODO - for items without skills

            Strategy = Config.UseStrategy.ProduceStrategy(Owner,Config, item);
        }

        public bool TryUseItem(out BaseUnitAction onUse)
        {
            bool ok = Strategy.TryUseItem(out onUse);
            return ok;
        }



        public void DoUpdate(float delta)
        {
            Strategy.Update(delta);
            EventBus<UpdateIconEvent>.Raise(new UpdateIconEvent(this, Owner));
        }


        #region UI


        public Sprite Icon => Config.Description.Picture;

        public float CurrentNumber => Strategy.CurrentNumber;

        public float MaxNumber => Strategy.MaxNumber;

        #endregion
    }

}
