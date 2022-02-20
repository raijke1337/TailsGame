using UnityEngine;

public class HealthCommandTrigger : BaseCommandTrigger
{
    private ChangeHealthCommand _comm;
    [SerializeField]
    private string _commandID;
    public string GetCommandID() => _commandID;
    
    protected override void Start()
    {
        base.Start();
        // todo doesnt inject properly and causes errors
        _comm = _manager.GetCommandByID<ChangeHealthCommand>(_commandID);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponent<Unit>();
        if (unit == null) return;

        var result = _comm.Clone();

        result.Target = unit;
        unit.GetUnitState.GetCommandsAssistant.AddCommand(result);

    }




}

