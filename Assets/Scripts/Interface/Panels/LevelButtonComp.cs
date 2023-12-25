using Arcatech.Scenes;
using TMPro;
using UnityEngine;
namespace Arcatech.UI
{
    public class LevelButtonComp : MonoBehaviour
    {
        [SerializeField] private SceneContainer _data;
        private TextMeshProUGUI _text;
        public event SimpleEventsHandler<SceneContainer> OnButtonClick;
        private RectTransform _rect;
        public Vector2 GetSize
        {
            get
            {
                if (_rect == null) _rect = GetComponent<RectTransform>();
                return _rect.sizeDelta;
            }
        }
        public SceneContainer LevelData
        {
            get
            {
                return _data;
            }
            set
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
                _text.text = value.Description.Title;
                _data = value;
            }
        }

        public void ClickedEventCall()
        {
            OnButtonClick?.Invoke(_data);
        }

    }
}