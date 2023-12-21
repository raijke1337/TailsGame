using Arcatech.Units;
using UnityEngine;
namespace Arcatech.UI
{
    [RequireComponent(typeof(BaseUnit))]
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