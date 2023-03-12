using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class RoomController : LoadedManagerBase
{
    private Collider _detectionArea;
    private PlayerUnit _player;
    private List<NPCUnit> _npcUnits = new List<NPCUnit>();

    private StateMachineUpdater _FSMupdater;
    public SimpleEventsHandler<NPCUnit> UnitFound;

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
        arg.ReactToDamage(_player);
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
    public BaseUnit GetUnitForAI(UnitType type)
    {
        BaseUnit res = null;
        switch (type)
        {
            case UnitType.Small:
                res = _npcUnits.ToList().FirstOrDefault(t => t.GetUnitType() == type);
                break;
            case UnitType.Big:
                res = _npcUnits.ToList().FirstOrDefault(t => t.GetUnitType() == type);
                break;
            case UnitType.Boss:
                res = _npcUnits.ToList().FirstOrDefault(t => t.GetUnitType() == type);
                break;
            case UnitType.Self:
                Debug.LogWarning(type + " was somehow requested, this should not happen");
                break;
            case UnitType.Any:
                Debug.LogWarning(type + " NYI");
                break;
            case UnitType.Player:
                res = _player;
                break;
        }
        return res;
    }

    #endregion


}


