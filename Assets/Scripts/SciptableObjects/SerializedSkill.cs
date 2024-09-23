using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
using UnityEngine.Assertions;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Skill Config", menuName = "Items/Skills/Skill")]
    public class SerializedSkill : ScriptableObject
    {
        [Header("Use strategy"),SerializeField] public SerializedSkillUseStrategy UseStrategy;
        [Header("Text"), SerializeField] public ExtendedText Description;
        [Header("Effects"), SerializeField] public SerializedEffectsCollection Effects;

        [Space, Header("Combat"), SerializeField]
        public UnitActionType UnitActionType;
        [SerializeField] public SerializedStatsEffectConfig Cost;


        public Skill CreateSkill(BaseEntity owner, BaseEquippableItemComponent item)
        {
            return new Skill(this,owner,item);
        }
        private void OnValidate()
        {
            Assert.IsNotNull(UseStrategy);
            Assert.IsNotNull(Cost);
        }
    }
}