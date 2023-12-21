using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Units
{
    [Serializable]
    public class SkillsController : BaseController, INeedsEmpties
    {


        public SkillEvents<EquipItemType> SwitchAnimationLayersEvent;
        public ItemEmpties Empties { get; }
        public SkillsController(ItemEmpties ie,BaseUnit Owner) : base (Owner)
        {
            Empties = ie;
        }




        // this is used in game for skill requests
        private Dictionary<CombatActionType, SkillObjectForControls> _skills;
//        private Dictionary<CombatActionType, ProjectileSettingsPackage> _projSkillsData;


        public void LoadItemSkill(EquipmentItem item)
        {
            if (item == null) return;

            if (_skills == null) _skills = new Dictionary<CombatActionType, SkillObjectForControls>();

            SkillObjectForControls control = new SkillObjectForControls(item.ItemSkillConfig,Owner);
            IsReady = true;
            switch (item.ItemType)
            {
                case EquipItemType.MeleeWeap:
                    _skills[CombatActionType.MeleeSpecialQ] = control;
                    break;
                case EquipItemType.RangedWeap:
                    _skills[CombatActionType.RangedSpecialE] = control;
                    break;
                case EquipItemType.Shield:
                    _skills[CombatActionType.ShieldSpecialR] = control;
                    break;
                case EquipItemType.Booster:
                    _skills[CombatActionType.Dodge] = control;
                    break;
                default:
                    break;                    
            }
        }



        public bool TryUseSkill(CombatActionType type, float CurrentCombo, out SkillComponent result)
        {
            result = null;
            var usedSkill = _skills[type];
           if ( usedSkill == null ) return false;

            switch (type)
            {
                case CombatActionType.Dodge:
                    
                    break;
                default:
                    if (CurrentCombo < usedSkill.Cost || !usedSkill.TryUse()) return false;
                    else
                    {
                        result = usedSkill.GetInstantiatedSkillCollider;
                        switch (type)
                        {
                            case CombatActionType.MeleeSpecialQ:
                                SwitchAnimationLayersEvent?.Invoke(EquipItemType.MeleeWeap);
                                break;
                            case CombatActionType.RangedSpecialE:
                                SwitchAnimationLayersEvent?.Invoke(EquipItemType.RangedWeap);
                                break;
                        }
                        return true;
                    }
            }
            return false;
        }

        public override void UpdateInDelta(float deltaTime)
        {
            if (_skills == null) return;

            foreach (var sk in _skills.Values)
            {
                sk.UpdateInDelta(deltaTime);
            }
        }

        public SkillObjectForControls GetControlData(CombatActionType t) => _skills[t];
        public SkillObjectForControls[] GetControlData() => _skills.Values.ToArray();

    }



}