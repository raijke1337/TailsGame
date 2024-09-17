using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Skills
{
    public class Skill : ISkill
    {
        #region interface
        public BaseEntity Owner { get ; set; }
        protected SerializedSkill Config { get; }
        public UnitActionType UseActionType => Config.UnitActionType;
        public StatsEffect GetCost => new(Config.CostTrigger);

        #endregion

        protected SkillUsageStrategy Strategy { get; }


        public Skill(SerializedSkill settings, BaseEntity owner, BaseEquippableItemComponent item)
        { 

            Config = settings;
            Owner = owner;
            if (settings == null) return; // placeholder maybe TODO - for items without skills

            Strategy = Config.UseStrategy.ProduceStrategy(Owner,Config, item);
        }

        public bool TryUseItem(out BaseUnitAction onUse)
        {
            bool ok = Strategy.TryUseUsable(out onUse);
            return ok;
        }



        public void DoUpdate(float delta)
        {
            Strategy.UpdateUsable(delta);
            EventBus<UpdateIconEvent>.Raise(new UpdateIconEvent(this, Owner));
        }


        #region UI


        public Sprite Icon => Config.Description.Picture;

        public float CurrentNumber => Strategy.CurrentNumber;

        public float MaxNumber => Strategy.MaxNumber;

        #endregion
    }

}
