public class ServiceGolemInputs : InputsNPC
{
    private DodgeController _dodgeCtrl;
    public void SetDodgeCtrl(string ID) => _dodgeCtrl = new DodgeController(ID);

    protected override void HandleAttackRequest()
    {
        if (!_dodgeCtrl.IsDodgePossibleCheck())
        {
            if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee))
            {
                CombatActionSuccessCallback(CombatActionType.Melee);
            }
        }
        else
        {
            CombatActionSuccessCallback(CombatActionType.Dodge);
        }
    }

    protected override void Fsm_CombatPreparationSM()
    {
    }

    public DodgeController GetDodgeController => _dodgeCtrl;



}

