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
        [SerializeField] int Charges;
        [SerializeField] float ChargeReload;

        [Space,Header("Usage")]
        [SerializeField] SerializedUnitAction OnUseAction;
        [SerializeField] SerializedActionResult[] SkillResultsOnUse;
        [SerializeField] SerializedActionResult[] SkillResultsOnCompleteAction;

        public virtual SkillUsageStrategy ProduceStrategy(BaseEntity owner,SerializedSkill cfg, BaseEquippableItemComponent item)
        {
            return new SkillUsageStrategy(SkillResultsOnUse, item, OnUseAction,owner,cfg,Charges,ChargeReload);
        }

        private void OnValidate()
        {
            Assert.IsNotNull(SkillResultsOnUse);
            Assert.IsNotNull(OnUseAction);
            Assert.IsFalse(Charges==0);
        }
    }



}