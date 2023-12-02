using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    [RequireComponent(typeof(Collider))]
    public class RoomController : LoadedManagerBase
    {
        private Collider _detectionArea;
        private PlayerUnit _player;
        private List<NPCUnit> _npcUnits = new List<NPCUnit>();

        private StateMachineUpdater _FSMupdater;
        public SimpleEventsHandler<NPCUnit> UnitFound;

        #region managed
        public override void Initiate()
        {
            _detectionArea = GetComponent<Collider>();
            _detectionArea.enabled = true;
            _detectionArea.isTrigger = true;
            StartCoroutine(DelayDetection());

            _player = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;
            _FSMupdater = new StateMachineUpdater();
        }

        public override void RunUpdate(float delta)
        {
            _FSMupdater.OnUpdate(delta);
        }

        public override void Stop()
        {
            foreach (var unit in _npcUnits)
            {
                _FSMupdater.RemoveUnit(unit);
            }
        }
        #endregion

        private IEnumerator DelayDetection()
        {
            yield return new WaitForSeconds(0.1f);
            if (_detectionArea.enabled) _detectionArea.enabled = false;
            yield return null;
        }

        #region units

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                var unit = other.GetComponent<NPCUnit>();
                //Debug.Log($"Added unit {unit} by {this}");
                UnitFound?.Invoke(unit);
                unit.SetUnitRoom(this);
                unit.BaseUnitDiedEvent += Unit_BaseUnitDiedEvent;
                unit.OnUnitAttackedEvent += Unit_OnUnitAttackedEvent;
                _FSMupdater.AddUnit(unit);
                _npcUnits.Add(unit);
            }

        }
        #region unit events

        private void Unit_OnUnitAttackedEvent(NPCUnit arg)
        {
            var otherUnits = new List<NPCUnit>();
            otherUnits.AddRange(_npcUnits);
            otherUnits.Remove(arg);
            var range = arg.GetStateMachine.GetEnemyStats.LookSpereCastRange;
            foreach (var u in otherUnits)
            {
                var dist = Vector3.Distance(arg.transform.position, u.transform.position);
                if (dist < range)
                {
                    u.ForceCombat();
                    Debug.Log($"{u.GetFullName} enters combat because {arg.GetFullName} was attacked, range {dist}");
                }
            }

        }


        private void Unit_BaseUnitDiedEvent(BaseUnit unit)
        {
            _FSMupdater.RemoveUnit(unit as NPCUnit);

            if (unit is NPCUnit n)
            {
                _npcUnits.Remove(n);
                n.OnUnitAttackedEvent -= Unit_OnUnitAttackedEvent;
            }
        }

        #endregion


        // used by inputs
        public BaseUnit GetUnitForAI(ReferenceUnitType type)
        {
            BaseUnit res = null;
            switch (type)
            {
                case ReferenceUnitType.Small:
                    res = _npcUnits.ToList().FirstOrDefault(t => t.GetUnitType == type);
                    break;
                case ReferenceUnitType.Big:
                    res = _npcUnits.ToList().FirstOrDefault(t => t.GetUnitType == type);
                    break;
                case ReferenceUnitType.Boss:
                    res = _npcUnits.ToList().FirstOrDefault(t => t.GetUnitType == type);
                    break;
                case ReferenceUnitType.Self:
                    Debug.LogWarning(type + " was somehow requested, this should not happen");
                    break;
                case ReferenceUnitType.Any:
                    Debug.LogWarning(type + " NYI");
                    break;
                case ReferenceUnitType.Player:
                    res = _player;
                    break;
            }
            return res;
        }

        #endregion


    }


}