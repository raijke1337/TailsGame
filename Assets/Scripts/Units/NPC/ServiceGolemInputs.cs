using UnityEngine;

public class ServiceGolemInputs : InputsNPC
{
    private DodgeController _dodgeCtrl;
    public void SetDodgeCtrl(string ID)
    {
        _dodgeCtrl = new DodgeController(ID);
        _handler.RegisterUnitForStatUpdates(_dodgeCtrl, true);
    }

    protected override void Bind(bool isStart)
    {
        base.Bind(isStart);
        if (!isStart) _handler.RegisterUnitForStatUpdates(_dodgeCtrl, false);
    }
    protected override void Fsm_AgressiveActionRequestSM(CombatActionType type)
    {
        bool inRange = _stateMachine.CheckIsInStoppingRange();

        switch (type)
        {
            case CombatActionType.Melee:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case CombatActionType.Ranged:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case CombatActionType.Dodge:
                if (inRange == false && _stateMachine.TimeInState >= _stateMachine.CurrentState.StateExpiryTime && _dodgeCtrl.IsDodgePossibleCheck())
                    CombatActionSuccessCallback(CombatActionType.Dodge);
                break;
            case CombatActionType.MeleeSpecialQ:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case CombatActionType.RangedSpecialE:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case CombatActionType.ShieldSpecialR:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
        }
    }

    protected override void Fsm_CombatPreparationSM()
    {
        Debug.Log($"{_statsCtrl.GetDisplayName} readied for combat!");
    }

    public DodgeController GetDodgeController => _dodgeCtrl;



}

