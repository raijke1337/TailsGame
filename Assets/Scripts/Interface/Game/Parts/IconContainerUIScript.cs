
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcatech.UI
{
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
        public TextMeshProUGUI Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text == null)
                {
                    _text = GetComponent<TextMeshProUGUI>();
                }
                _text = value;
            }
        }
    }
}