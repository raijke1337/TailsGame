using Arcatech.Skills;
using KBCore.Refs;
using UnityEngine;

namespace Arcatech.Units
{
    [RequireComponent(typeof(WeaponController), typeof(SkillsController))]
    public abstract class ArmedUnit : DummyUnit
    {


        [SerializeField, Self] protected WeaponController _weapons;
        [SerializeField, Self] protected SkillsController _skills;

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();

            _weapons.UpdateWeapons(_inventory.GetWeapons)
                .SetInventory(_inventory)
                .SetStats(_stats)
                .StartController();

            _skills.SetSkills(_inventory.GetSkillConfigs)
                .SetStats(_stats)
                .StartController();

        }

        public override void DisableUnit()
        {
            base.DisableUnit();
            _weapons.StopController();
            _skills.StopController();
        }

        public override void RunFixedUpdate(float delta)
        {

            base.RunFixedUpdate(delta);
            _skills.FixedControllerUpdate(delta);
            _weapons.FixedControllerUpdate(delta);
        }
        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _skills.ControllerUpdate(delta);
            _weapons.ControllerUpdate(delta);

        }
    }


}