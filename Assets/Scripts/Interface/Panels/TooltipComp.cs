using Arcatech.Items;
using TMPro;
using UnityEngine;
namespace Arcatech.UI
{
    public class TooltipComp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _desc;
        private RectTransform _rect;

        public RectTransform GetRect { get => _rect; }

        public void SetTexts(InventoryItem item)
        {
            if (_name == null | _desc == null)
            {
                Debug.LogWarning("Set fields for " + this);
                return;
            }
            if (item == null) return;
            _name.text = item.GetDisplayName;
            _desc.text = item.GetDescription;
        }
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }



    }
}