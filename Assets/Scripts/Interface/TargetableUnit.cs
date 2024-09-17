using Arcatech.Stats;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.UI
{
    [RequireComponent(typeof(BaseEntity))]
    public class TargetableUnit : BaseTargetableItem
    {
        public StatValueContainer GetHealthStat { get => _hp; }
        private StatValueContainer _hp;

        protected override void LookUpValuesOnActivation()
        {
            if (TryGetComponent<BaseEntity>(out var u))
            {
                //_title = u.GetFullName;
                //_hp = u.GetStats[BaseStatType.Health];
            }
        }
    }
}