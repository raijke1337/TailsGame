public class CrawlerMechInputs : InputsNPC
{
    // find ally and run to them, then fight

    protected override void Fsm_AgressiveActionRequestSM(CombatActionType type)
    {
        base.Fsm_AgressiveActionRequestSM(type);
    }

    protected override void Fsm_AggroRequestedSM()
    {
        _stateMachine.FocusUnit = _stateMachine.StateMachineUnit; // for self-destruct skill
        base.Fsm_AggroRequestedSM();
    }

}

