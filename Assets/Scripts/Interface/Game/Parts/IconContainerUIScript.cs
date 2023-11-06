
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
                _text = value; 
            }
        }
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _image = GetComponent<Image>();
        }
    }
}