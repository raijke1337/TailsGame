using Arcatech.Actions;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Skill use Logic", menuName = "Items/Skills/SkillUseLogic")]
    public class SerializedSkillUseStrategy : ScriptableObject
    {
        [Header("Cooldowns")]
        public int Charges;
        public float ChargeReload;

        [Space] public SerializedUnitAction OnUseAction;
        public SerializedActionResult[] SkillResults;

        public virtual SkillUsageStrategy ProduceStrategy(BaseUnit owner,SerializedSkill cfg, BaseEquippableItemComponent item)
        {
            return new SkillUsageStrategy(SkillResults, item, OnUseAction,owner,cfg,Charges,ChargeReload);
        }
    }



}