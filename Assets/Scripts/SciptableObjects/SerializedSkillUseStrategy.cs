using Arcatech.Actions;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
using UnityEngine.Assertions;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Skill use Logic", menuName = "Items/Skills/SkillUseLogic")]
    public class SerializedSkillUseStrategy : ScriptableObject
    {
        [Header("Cooldowns")]
        public int Charges;
        public float ChargeReload;

        [Space] public SerializedUnitAction OnUseAction;
        public SerializedActionResult[] SkillResultsOnUse;
        public SerializedActionResult[] SkillResultsOnCompleteAction;

        public virtual SkillUsageStrategy ProduceStrategy(BaseEntity owner,SerializedSkill cfg, BaseEquippableItemComponent item)
        {
            return new SkillUsageStrategy(SkillResultsOnUse, item, OnUseAction,owner,cfg,Charges,ChargeReload);
        }

        private void OnValidate()
        {
            Assert.IsNotNull(SkillResultsOnUse);
            Assert.IsNotNull(OnUseAction);
            //Assert.IsNotNull(SkillResultsOnCompleteAction);
            Assert.IsFalse(Charges==0);
        }
    }



}