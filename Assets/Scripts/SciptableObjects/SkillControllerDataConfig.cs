using UnityEngine;
namespace Arcatech
{
    [CreateAssetMenu(fileName = "New SkillControllerDataConfig", menuName = "Configurations/Skills")]
    public class SkillControllerDataConfig : ScriptableObjectID
    {
        public CombatActionType SkillType;
        public SkillData Data;
    }


}