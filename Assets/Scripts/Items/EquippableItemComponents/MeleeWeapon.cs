using Arcatech.Managers;
using Arcatech.Triggers;
using UnityEngine;
namespace Arcatech.Items
{
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

            try
            {
                GameManager.Instance.GetGameControllers.TriggersProjectilesManager.RegisterTrigger(_trigger);
            }
            catch
            {
                // this is probably a scene
                // if the trifggers manager is not started this registering is not necessary
            }

            
        }

        public void ToggleColliders(bool enable)
        {
            _trigger.ToggleCollider(enable);
        }



    }

}