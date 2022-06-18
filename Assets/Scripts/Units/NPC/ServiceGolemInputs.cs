using UnityEngine;

public class ServiceGolemInputs : InputsNPC
{
    private DodgeController _dodgeCtrl;
    public void SetDodgeCtrl(string ID) => _dodgeCtrl = new DodgeController(ID);

    protected override void Bind(bool isStart)
    {
        base.Bind(isStart);
        _handler.RegisterUnitForStatUpdates(_dodgeCtrl, isStart);
    }
    protected override void HandleAttackRequest(CombatActionType type)
    {
        //todo
        bool inRange = fsm.CheckInStoppingRange();

        if (!inRange && fsm.TimeInState >= 1f) // todo configs
        {
            if (_dodgeCtrl.IsDodgePossibleCheck())
            CombatActionSuccessCallback(CombatActionType.Dodge);
        }
        else if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee) && inRange)
        {
            CombatActionSuccessCallback(CombatActionType.Melee);
        }

    }

    protected override void Fsm_CombatPreparationSM()
    {
        Debug.Log($"{_statsCtrl.GetDisplayName} readied for combat!");
    }

    public DodgeController GetDodgeController => _dodgeCtrl;



}

