using Arcatech.EventBus;

namespace Arcatech.Units
{
    public struct InventoryUpdateEvent : IEvent
    {
        public InventoryUpdateEvent(DummyUnit unit, UnitInventoryComponent inventory)
        {
            Unit = unit;
            Inventory = inventory;
        }

        public DummyUnit Unit { get; }
        public UnitInventoryComponent Inventory { get; }
    }
}