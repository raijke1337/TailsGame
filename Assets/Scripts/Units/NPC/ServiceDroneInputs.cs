namespace Arcatech.Units.Inputs
{

    public class ServiceDroneInputs : InputsNPC
    {
        protected override void Fsm_ChangeRangeActionRequestSM(CombatActionType arg)
        {
            switch (arg)
            {
                case CombatActionType.Melee:
                    break;
                case CombatActionType.Ranged:
                    _stateMachine.NMAgent.stoppingDistance = _enemyStats.AttackRange;
                    break;
                case CombatActionType.Dodge:
                    break;
                case CombatActionType.MeleeSpecialQ:
                    break;
                case CombatActionType.RangedSpecialE:
                    _stateMachine.NMAgent.stoppingDistance = _skillCtrl.GetControlData(CombatActionType.RangedSpecialE).EffectRadius ;
                    break;
                case CombatActionType.ShieldSpecialR:
                    break;
            }
        }
    }
}