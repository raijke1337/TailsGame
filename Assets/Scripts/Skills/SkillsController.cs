using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Skills
{
    public class SkillsController : ManagedControllerBase, ICombatActions
    {

        protected Dictionary<UnitActionType, ISkill> _skills;

        public SkillsController (UnitInventoryController inv, DummyUnit ow) : base (ow)
        {
            _skills = new();
            foreach (var skill in inv.GetSkillConfigs)
            {
                _skills[skill.UnitActionType] = new SkillObjectForControls(skill);
            }
        }

        #region  interface
               

        public bool TryUseAction(UnitActionType action)
        { 
            if (_skills.ContainsKey(action))
            {
                _skills[action].TryUseItem();
                return true;
            }
            return false;

        }

        #endregion
        public override void StartController()
        {

        }

        public override void ControllerUpdate(float delta)
        {
            //throw new NotImplementedException();
        }

        public override void FixedControllerUpdate(float fixedDelta)
        {
            //throw new NotImplementedException();
        }

        public override void StopController()
        {
            //throw new NotImplementedException();
        }

    }
}