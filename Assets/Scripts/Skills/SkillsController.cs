using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Skills
{
    public class SkillsController : ManagedControllerBase, ICombatActions, INeedsOwner
    {

        protected Dictionary<UnitActionType, ISkill> _skills;
        UnitStatsController _stats;
        UnitInventoryComponent _inventory;


        public SkillsController SetSkills(SerializedSkillConfiguration[] ss)
        {
            _skills = new();
            foreach (var skill in ss)
            {
                _skills[skill.UnitActionType] = new SkillObjectForControls(skill);
            }
            return this;
        }

        #region  interface
               
        public IUsesStats SetStats(UnitStatsController s)
        {
            _stats = s;
            return this;
        }

        public bool TryUseAction(UnitActionType action)
        { 
            if (_skills.ContainsKey(action))
            {
                _skills[action].UseItem();
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

        public IUsesStats SetInventory(UnitInventoryComponent comp)
        {
            _inventory = comp;
            return this;
        }
    }
}