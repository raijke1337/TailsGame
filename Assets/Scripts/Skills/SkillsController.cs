using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcatech.Units
{
    [Serializable]
    public class SkillsController : BaseController, INeedsEmpties
    {


        public SkillEvents<EquipmentType> SwitchAnimationLayersEvent;
        public ItemEmpties Empties { get; }
        public SkillsController(ItemEmpties ie, BaseUnit Owner) : base(Owner)
        {
            Empties = ie;
        }

        // this is used in game for skill requests
        private Dictionary<UnitActionType, SkillObjectForControls> _skills;

        public void LoadItemSkill(EquipmentItem item)
        {
            if (item == null) return;

            if (_skills == null) _skills = new Dictionary<UnitActionType, SkillObjectForControls>();

            var cfg = item.Skill;
            SkillObjectForControls control = new SkillObjectForControls(cfg, Owner);

            if (cfg is DodgeSkillConfigurationSO dcfg)
            {
                control = new DodgeSkillObjectForControls(dcfg, Owner);
            }

            
            IsReady = true;
            switch (item.ItemType)
            {
                case EquipmentType.MeleeWeap:
                    _skills[UnitActionType.MeleeSkill] = control;
                    break;
                case EquipmentType.RangedWeap:
                    _skills[UnitActionType.RangedSkill] = control;
                    break;
                case EquipmentType.Shield:
                    _skills[UnitActionType.ShieldSkill] = control;
                    break;
                case EquipmentType.Booster:
                    _skills[UnitActionType.DodgeSkill] = control;
                    break;
                default:
                    break;
            }
        }


        public bool IsSkillReady (UnitActionType actionType, out StatsEffect cost)
        {
            cost = null;
            if (_skills == null || _skills[actionType] == null) return false;

            else
            {
                cost = _skills[actionType].Cost;
                return _skills[actionType].SkillCooldownReady;
            }
        }
        public SkillProjectileComponent ProduceSkill(UnitActionType actionType)
        {
            var result = _skills [actionType].UseSkill;

                        switch (actionType)
                        {
                            case UnitActionType.MeleeSkill:
                                SwitchAnimationLayersEvent?.Invoke(EquipmentType.MeleeWeap);
                                break;
                            case UnitActionType.RangedSkill:
                                SwitchAnimationLayersEvent?.Invoke(EquipmentType.RangedWeap);
                                break;
                        }
             SpawnProjectileCallBack(result);
             //result.transform.position = Owner.GetEmpties.ItemPositions[result.SpawnPlace].position;
             //           result.transform.forward = Owner.transform.forward;
            return result;
        }




public SkillObjectForControls GetControlData(UnitActionType t) => _skills[t];
        public SkillObjectForControls[] GetControlData() => _skills.Values.ToArray();


        public override void UpdateInDelta(float deltaTime)
        {
            if (_skills == null) return;

            foreach (var sk in _skills.Values)
            {
                sk.UpdateInDelta(deltaTime);
            }
        }

        public override void ApplyEffect(StatsEffect effect)
        {
           // throw new NotImplementedException();
        }

        public override void StartComp()
        {
            //throw new NotImplementedException();
        }

        public override void StopComp()
        {
            //throw new NotImplementedException();
        }
    }
}