using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.UI
{
    public class TargetableUnit : BaseTargetableItem
    {
        public StatValueContainer GetHealthStat { get => _hp; }
        private StatValueContainer _hp;

        protected override void LookUpValuesOnActivation()
        {
            if (TryGetComponent<BaseUnit>(out var u))
            {
                _title = u.GetFullName;
                _hp = u.GetStats[BaseStatType.Health];
            }
        }
    }
}