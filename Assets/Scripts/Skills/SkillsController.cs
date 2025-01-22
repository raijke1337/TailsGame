using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Units;
using System;
using System.Collections.Generic;

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
            onUse = null;
            if (_skills.ContainsKey(action))
            {
                bool ok = _skills[action].TryUseItem(stats, out onUse);
                if (ok)
                {
                    inv.DrawItems(_skills[action].DrawStrategy);
                    return ok;
                }
            }
            return false;
        }

        public bool CanUseAction(UnitActionType action)
        {
            try
            {
                return _skills[action].CanUseItem(stats);
            }
            catch
            { return false; }
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