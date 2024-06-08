using Arcatech.Managers;
using Arcatech.Units;
using Arcatech.Units.Inputs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.AI
{
    public class RoomUnitsGroup
    {
        public List<NPCUnit> GetAllUnits => _units;
        private List<NPCUnit> _units;
        public EnemiesLevelBlockDecorComponent SpawnRoom { get; set; }

        public RoomUnitsGroup(List<NPCUnit> list)
        {
            _units = list;
            foreach (BaseUnit unit in _units)
            {
                unit.BaseUnitDiedEvent += RemoveUnitOnDeath;
            }
            foreach (NPCUnit unit in _units)
            {
                unit.OnUnitAttackedEvent += Unit_OnUnitAttackedEvent;
                unit.UnitsGroup = this;
            }
        }

        private void Unit_OnUnitAttackedEvent(NPCUnit arg)
        {
            var otherUnits = new List<NPCUnit>();
            otherUnits.AddRange(_units);
            otherUnits.Remove(arg);

            foreach (var u in otherUnits)
            {
                var inputs = u.GetInputs<InputsNPC>();
                inputs.ForceCombat();
                if (inputs.DebugMessage)
                {
                    Debug.Log($"{u.GetFullName} enters combat because {arg.GetFullName} was attaked!");
                }
            }

        }


        private void RemoveUnitOnDeath(BaseUnit u)
        {
            if (u is NPCUnit unit)
            {
                _units.Remove(unit);
                unit.BaseUnitDiedEvent -= RemoveUnitOnDeath;
                unit.OnUnitAttackedEvent -= Unit_OnUnitAttackedEvent;
            }
        }


        // used by inputs
        public BaseUnit GetUnitForAI(ReferenceUnitType type)
        {
            BaseUnit res = null;
            switch (type)
            {
                case ReferenceUnitType.Small:
                    res = _units.FirstOrDefault(t => t.GetUnitType() == type);
                    break;
                case ReferenceUnitType.Big:
                    res = _units.FirstOrDefault(t => t.GetUnitType() == type);
                    break;
                case ReferenceUnitType.Boss:
                    res = _units.FirstOrDefault(t => t.GetUnitType() == type);
                    break;
                case ReferenceUnitType.Self:
                    Debug.LogWarning(type + " was somehow requested, this should not happen");
                    break;
                case ReferenceUnitType.Any:
                    Debug.LogWarning(type + " NYI");
                    break;
                case ReferenceUnitType.Player:
                    res = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;
                    break;
            }
            return res;
        }
    }
}