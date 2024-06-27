using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Arcatech.Skills
{

    [Serializable]
    public class SkillObjectForControls : ISkill
    {
        public DummyUnit Owner { get ; set; }

        private SerializedSkillConfiguration _settings;

        public StatsEffect GetCost => new(_settings.CostTrigger);

        public UnitActionType UseActionType => throw new NotImplementedException();

        private BaseSkillUsageStrategy _strat;

        public SkillObjectForControls(SerializedSkillConfiguration settings)
        { 
            _settings = settings;
        }
        public SkillObjectForControls SetOwner(DummyUnit owner)
        {
            Owner = owner;
            return this;
        }

        public IUsableItem AssignStrategy()
        {
            _strat = new ProjectileSkillStrategy(_settings.SkillProjectileConfig.ProjectilePrefab);
            return this; 
        }
        public void UseItem()
        {
            Debug.Log($"use skill {_settings.Description.Title}");
            _strat.SkillUseStateEnter();
        }
    }

}
