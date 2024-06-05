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
            foreach (NPCUnit unit in _units)
            {
                unit.BaseUnitDiedEvent += RemoveUnitOnDeath;
                unit.OnUnitAttackedEvent += Unit_OnUnitAttackedEvent;
                unit.UnitsGroup = this;
            }
        }

        private void Unit_OnUnitAttackedEvent(NPCUnit arg)
        {
            Debug.Log($"{arg} from group {SpawnRoom.name} was attacked");

            //var otherUnits = new List<NPCUnit>();
            //otherUnits.AddRange(_units);
            //otherUnits.Remove(arg);
            //var range = arg.stat
            //foreach (var u in otherUnits)
            //{
            //    var dist = Vector3.Distance(arg.transform.position, u.transform.position);
            //    if (dist < range)
            //    {
            //        u.ForceCombat();
            //        //Debug.Log($"{u.GetFullName} enters combat because {arg.GetFullName} was attacked, range {dist}");
            //    }
            //}

        }


        private void RemoveUnitOnDeath(BaseUnit u)
        { 
            if (u is NPCUnit unit)
            {
                _units.Remove(unit);
                unit.OnUnitAttackedEvent -= RemoveUnitOnDeath;
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