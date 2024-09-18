using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units;

namespace Arcatech
{
    public interface IUsablesStrategy : IStrategy
    {
        BaseEntity Owner { get; }
        bool TryUseUsable(out BaseUnitAction action);
        void UpdateUsable(float delta);
    }

    public interface IShieldAbsorbStrategy
    {
        public StatsEffect[] SplitDamage (StatsEffect damage, StatValueContainer shieldCharge);
    }
    public interface IStrategy
    {
    }
}