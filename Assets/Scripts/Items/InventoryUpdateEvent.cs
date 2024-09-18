using Arcatech.EventBus;
using Arcatech.Units;

namespace Arcatech.Items
{
    public struct InventoryUpdateEvent : IEvent
    {
        public InventoryUpdateEvent(EquippedUnit unit, UnitInventoryController inventory)
        {
            Unit = unit;
            Inventory = inventory;
        }

        public EquippedUnit Unit { get; }
        public UnitInventoryController Inventory { get; }
    }
}