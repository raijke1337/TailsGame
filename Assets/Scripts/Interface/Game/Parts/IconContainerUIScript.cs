
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcatech.UI
{
    [RequireComponent(typeof(TextMeshProUGUI), typeof(Image))]
    public class IconContainerUIScript : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image == null) _image = GetComponent<Image>();
                _image.sprite = value.sprite;
            }
        }
        public string Text
        {
            get
            {
                if (_text == null)
                {
                    _text = GetComponent<TextMeshProUGUI>();
                }
                return _text.text;
            }
            set
            {
                if (_text == null)
                {
                    _text = GetComponent<TextMeshProUGUI>();
                }
                _text.text = value;
            }
        }
    }
}