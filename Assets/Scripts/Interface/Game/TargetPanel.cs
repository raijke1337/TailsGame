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
            Debug.Log($"assign item {item} to tgt panel : NYI");
            //if (item != _currentItem && _currentItem != null)
            //{
            //    _currentItem.ToggleCam(false);
            //}

            //_currentItem = item;
            //_description.text = item.GetTitle;
            //_image.texture = item.ToggleCam(true);


            //if (item is TargetableUnit u)
            //{
                
            //    _bars.gameObject.SetActive(true);
            //    _bars.LoadValues(u.GetHealthStat, DisplayValueType.Health);
            //}
            //else
            //{
            //    _bars.gameObject.SetActive(false);
            //}
        }
 
    }

}