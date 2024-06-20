namespace Arcatech.Units.Inputs
{

    public class ServiceDroneInputs : InputsNPC
    {
        protected override void Fsm_ChangeRangeActionRequestSM(UnitActionType arg)
        {
            switch (arg)
            {
                case UnitActionType.Melee:
                    break;
                case UnitActionType.Ranged:
                    _stateMachine.NMAgent.stoppingDistance = EnemyStats.AttackRange;
                    break;
                case UnitActionType.DodgeSkill:
                    break;
                case UnitActionType.MeleeSkill:
                    break;
                case UnitActionType.RangedSkill:
                    _stateMachine.NMAgent.stoppingDistance = _skillCtrl.GetControlData(UnitActionType.RangedSkill).EffectRadius;
                    break;
                case UnitActionType.ShieldSkill:
                    break;
            }
        }
    }
}