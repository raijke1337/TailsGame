using Arcatech.Units;
namespace Arcatech.Items
{
    public class Shield : Equipment
    {
        public Shield(ShieldSO cfg, EquippedUnit ow) : base(cfg, ow)
        {
            AbsorbStrategy = cfg.absorbStrategy.ProduceStrat();
        }
        public ShieldAbsorbStrategy AbsorbStrategy { get; }
    }
}