using Arcatech.EventBus;
using Arcatech.Units.Inputs;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class TargetPanel : PanelWithBarGeneric
    {

        [SerializeField,Child] protected TextMeshProUGUI _description;
        public void UpdateTargeted(bool show, ITargetable tgt)
        {
            if (!show)
            {
                _bars.ClearAllBars();
            }
            else
            {
                _description.text = tgt.GetName;
                foreach(var bar in tgt.GetDisplayValues)
                {
                    _bars.UpdateBarValue(bar.Key,bar.Value);
                }
            }
        }


    }

}