using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Texts;
using Arcatech.Units;
using KBCore.Refs;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.UI
{
    public class PlayerUnitPanel : ValidatedMonoBehaviour
    {
        [SerializeField,Child] protected IconContainersManager _icons;
        [SerializeField,Child] protected BarsContainersManager _bars;

        EventBinding<IUsableUpdatedEvent> bindIcons;

        private void Awake()
        {
            bindIcons = new EventBinding<IUsableUpdatedEvent>(RefreshIcon);
            EventBus<IUsableUpdatedEvent>.Register(bindIcons);
        }



        public void ShowStat (BaseStatType type , StatValueContainer cont) => _bars.UpdateBarValue(type, cont);
        public void ShowIcons (UnitInventoryController inv)
        {
            foreach (var weapon in inv.GetWeapons)
            {
                _icons.TrackIcon(weapon);
            }
        }
        private void RefreshIcon(IUsableUpdatedEvent obj)
        {
            if (obj.User is PlayerUnit)
            {
                _icons.TrackIcon(obj.Used);
            }
        }



        private void OnDisable()
        {
            EventBus<IUsableUpdatedEvent>.Deregister(bindIcons);
        }

    }

}