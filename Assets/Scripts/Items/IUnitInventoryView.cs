using UnityEngine.Events;

namespace Arcatech.Items
{
    public interface IUnitInventoryView
    {
        public event UnityAction InventoryChanged;
        void RefreshView (UnitInventoryModel model);

    }
}