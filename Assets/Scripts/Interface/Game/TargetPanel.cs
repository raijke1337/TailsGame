using Arcatech.Units;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class TargetPanel : PanelWithBarGeneric
    {

        [SerializeField] protected TextMeshProUGUI _description;
        [SerializeField] protected RawImage _image;

        public virtual void AssignItem(BaseTargetableItem item)
        {
            _description.text = item.GetTitle;
            item.ToggleCam(true);

            if (item is TargetableUnit u)
            {
                _bars.gameObject.SetActive(true);
                _bars.LoadValues(u.GetHealthStat, DisplayValueType.Health);
            }
            else
            {
                _bars.gameObject.SetActive(false);
            }
        }
        public override void StartController()
        {
            base.StartController(); // instantiate bars
        }

        public override void StopController()
        {
            _bars.StopController();
        }

        public override void UpdateController(float delta)
        {
            _bars.UpdateController(delta);
        }
    }

}