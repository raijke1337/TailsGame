using Arcatech.Units.Inputs;

public class ServiceGolemInputs : InputsNPC
{

    protected override void Fsm_AgressiveActionRequestSM(UnitActionType type)
    {
        bool inRange = _stateMachine.CheckIsInStoppingRange();

        switch (type)
        {
            case UnitActionType.Melee:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case UnitActionType.Ranged:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case UnitActionType.DodgeSkill:
               // if (inRange == false && _stateMachine.TimeInState >= _stateMachine.CurrentState.StateExpiryTime && _dodgeCtrl.IsReady) // maybe TODO
                    base.Fsm_AgressiveActionRequestSM(type);
                break;
            case UnitActionType.MeleeSkill:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case UnitActionType.RangedSkill:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
            case UnitActionType.ShieldSkill:
                base.Fsm_AgressiveActionRequestSM(type);
                break;
        }
    }

}


