using Arcatech.EventBus;
using Arcatech.Units;

namespace Arcatech.Items
{
    public struct InventoryUpdateEvent : IEvent
    {
        public InventoryUpdateEvent(DummyUnit unit, UnitInventoryController inventory)
        {
            Unit = unit;
            Inventory = inventory;
        }

        public DummyUnit Unit { get; }
        public UnitInventoryController Inventory { get; }
    }
}