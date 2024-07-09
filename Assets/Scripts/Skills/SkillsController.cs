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
        UnitInventoryController inv;
        UnitStatsController stats;
        protected Dictionary<UnitActionType, ISkill> _skills;

        public SkillsController (UnitStatsController stats, UnitInventoryController inv, DummyUnit ow) : base (ow)
        {
            this.inv = inv;
            this.stats = stats;

            _skills = new();

            foreach (var skill in inv.GetSkills)
            {
                _skills[skill.UseActionType] = skill;
            }
        }

        #region  interface


        public bool TryUseAction(UnitActionType action)
        {
            if (_skills.ContainsKey(action))
            {
                var cost = _skills[action].GetCost;
                if (cost == null || stats.TryApplyCost(cost))
                {
                    return _skills[action].TryUseItem();
                }
            }
            return false;
        }
        #endregion
        public override void StartController()
        {
        }

        public override void ControllerUpdate(float delta)
        {
            foreach (var s in _skills.Values) s.DoUpdate(delta); 
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