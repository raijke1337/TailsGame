using Arcatech.Items;
using Arcatech.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcatech.Units
{
    [Serializable]
    public class SkillsController : BaseController, INeedsEmpties
    {


        public SkillEvents<EquipItemType> SwitchAnimationLayersEvent;
        public ItemEmpties Empties { get; }
        public SkillsController(ItemEmpties ie, BaseUnit Owner) : base(Owner)
        {
            Empties = ie;
        }

        // this is used in game for skill requests
        private Dictionary<CombatActionType, SkillObjectForControls> _skills;

        public void LoadItemSkill(EquipmentItem item)
        {
            if (item == null) return;

            if (_skills == null) _skills = new Dictionary<CombatActionType, SkillObjectForControls>();

            var cfg = item.Skill;
            SkillObjectForControls control = new SkillObjectForControls(cfg, Owner);

            if (cfg is DodgeSkillConfigurationSO dcfg)
            {
                control = new DodgeSkillObjectForControls(dcfg, Owner);
            }

            
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



        public bool TryUseSkill(CombatActionType type, ComboController comboctrl, out SkillProjectileComponent result)
        {
            result = null;
            if (_skills.TryGetValue(type, out var usedSkill)) // check if a skill of TYPE is available
            {
                if (usedSkill.TryUseSkill(out var ef)) // not on cd, produces the combo cost trigger
                {
                    if (comboctrl.GetComboContainer.GetCurrent >= -ef.InitialValue) // enough combo to use, - neg because it is -15 in cfg
                    {
                        comboctrl.ApplyEffectToController(ef);
                        result = usedSkill.UseSkill;

                        switch (type)
                        {
                            case CombatActionType.MeleeSpecialQ:
                                SwitchAnimationLayersEvent?.Invoke(EquipItemType.MeleeWeap);
                                break;
                            case CombatActionType.RangedSpecialE:
                                SwitchAnimationLayersEvent?.Invoke(EquipItemType.RangedWeap);
                                break;
                        }
                        SpawnProjectileCallBack(result);
                        result.transform.position = Owner.GetEmpties.ItemPositions[result.SpawnPlace].position;
                        result.transform.forward = Owner.transform.forward;

                        return true;
                    }
                }
            }
            return false;
        }



        public SkillObjectForControls GetControlData(CombatActionType t) => _skills[t];
        public SkillObjectForControls[] GetControlData() => _skills.Values.ToArray();
        public override string GetUIText { get => ""; } // cooldowns are tracked in skill control datas


        public override void UpdateInDelta(float deltaTime)
        {
            if (_skills == null) return;

            foreach (var sk in _skills.Values)
            {
                sk.UpdateInDelta(deltaTime);
            }
        }

    }
}