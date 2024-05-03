using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Skills
{
    public class BoosterSkillInstanceComponent : SkillProjectileComponent
    {
        // stage 1 placer = find unit
        // move for x 
        // stage 2 effect = check wall collisions
        private void Start()
        {
            Destroy(gameObject,0.1f); // placeholder
        }

       public void InitializeDodgeSettings (DodgeSkillConfigurationSO cfg)
        {
            _data = new DodgeSettingsPackage(cfg);
        }

        private DodgeSettingsPackage _data;
        public DodgeSettingsPackage GetDodgeSettings
        {
            get => _data;
        }


    }
}