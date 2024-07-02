using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Texts;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.UI
{
    public class PlayerUnitPanel : ValidatedMonoBehaviour, IUnitInventoryView
    {
        [SerializeField,Child] protected IconContainersManager _icons;
        [SerializeField,Child] protected BarsContainersManager _bars;

        public event UnityAction InventoryChanged;

        public void RefreshView(UnitInventoryModel model)
        {
            Debug.Log($"load {model} into {this}");
        }

        public void ShowStat (BaseStatType type , StatValueContainer cont) => _bars.UpdateBarValue(type, cont);


    }

}