using TMPro;
using UnityEngine;
namespace Arcatech.UI
{
    public class LevelButtonComp : MonoBehaviour
    {
        private LevelData _data;
        private TextMeshProUGUI _text;
        public event SimpleEventsHandler<LevelButtonComp> OnButtonClick;
        private RectTransform _rect;
        public Vector2 GetSize
        {
            get
            {
                if (_rect == null) _rect = GetComponent<RectTransform>();
                return _rect.sizeDelta;
            }
        }
        public LevelData LevelData
        {
            get
            {
                return _data;
            }
            set
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
                _text.text = value.LevelNameShort;
                _data = value;
            }
        }

        public void ClickedEventCall()
        {
            OnButtonClick?.Invoke(this);
        }

    }
}