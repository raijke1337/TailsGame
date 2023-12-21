using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class TargetPanel : PanelWithBarGeneric
    {

        [SerializeField] protected TextMeshProUGUI _description;
        [SerializeField] protected RawImage _image;
        private BaseTargetableItem _currentItem;

        public virtual void AssignItem(BaseTargetableItem item)
        {
            if (item != _currentItem && _currentItem != null)
            {
                _currentItem.ToggleCam(false);
            }

            _currentItem = item;
            _description.text = item.GetTitle;
            _image.texture = item.ToggleCam(true);


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