using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Skills
{
    public abstract class SerializedSkillUseStrategy : ScriptableObject
    {
        [Header("Cooldowns")]
        public int Charges;
        public int Reload;
        public int CD;
        public SerializedUnitAction OnUseAction;    

        public virtual BaseSkillUsageStrategy ProduceStrategy(BaseUnit owner,SerializedSkill cfg, BaseEquippableItemComponent item)
        {
            return new BaseSkillUsageStrategy(item,OnUseAction,owner,cfg,Charges,Reload,CD);
        }
    }
}