using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Skills
{
    [Serializable]
    public class SkillsController : ManagedControllerBase, ICombatActions
    {
        UnitInventoryController inv;
        UnitStatsController stats;
        protected Dictionary<UnitActionType, ISkill> _skills;

        public SkillsController (UnitStatsController stats, UnitInventoryController inv, EquippedUnit ow) : base (ow)
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


        public bool TryUseAction(UnitActionType action, out BaseUnitAction onUse)
        {
            if (_skills.ContainsKey(action))
            {
                return _skills[action].TryUseItem(stats, out onUse);
            }
            onUse = null;
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
        }

        public override void StopController()
        {
        }

    }
}