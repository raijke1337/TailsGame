using Arcatech.Stats;
using Arcatech.Units;

namespace Arcatech
{
    public interface IUsesStats : IManagedController
    {
        public IUsesStats SetStats(UnitStatsController s);
        public IUsesStats SetInventory(UnitInventoryComponent comp);

    }


}