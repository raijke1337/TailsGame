using System.Collections.Generic;

public class UnitActivityHandler
{
    private List<NPCUnit> _units;
    private PlayerUnit _player;

    public UnitActivityHandler(IEnumerable<NPCUnit> units, PlayerUnit player)
    {
        _units = new List<NPCUnit>(); _player = player;
        _units.AddRange(units);
    }

    //public void SetAIStateGlobal(bool isProcessing)
    //{
    //    foreach (var npc in _units)
    //    {
    //        SetAIStateUnit(isProcessing, npc);
    //    }
    //}
    //public void SetAIStateUnit(bool isProcessing, NPCUnit unit)
    //{
    //    unit.GetInputs<InputsNPC>().SetAI(isProcessing);
    //    unit.GetInputs<InputsNPC>().NavMeshAg.isStopped = isProcessing;
    //}



}


