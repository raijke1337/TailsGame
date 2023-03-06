using UnityEngine;

[RequireComponent(typeof(WeaponTriggerComponent))]
public class MeleeWeapon : BaseWeapon
{
    private WeaponTriggerComponent _trigger;

    public override void SetUpWeapon(BaseWeaponConfig config)
    {
        base.SetUpWeapon(config);
        _trigger = GetComponent<WeaponTriggerComponent>();
        _trigger.SetTriggerIDS(_effectsIDs);
        _trigger.ToggleCollider(false);
        _trigger.Owner = Owner;

        GameManager.Instance.GetGameControllers.TriggersProjectilesManager.RegisterTrigger(_trigger);
    }

    public void ToggleColliders(bool enable)
    {
        _trigger.ToggleCollider(enable);
    }



}

