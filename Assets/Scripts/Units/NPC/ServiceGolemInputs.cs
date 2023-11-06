using Arcatech.Units.Inputs;

public class ServiceGolemInputs : InputsNPC
{

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

}


