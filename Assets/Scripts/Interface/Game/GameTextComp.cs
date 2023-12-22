using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcatech
{
    public class GameTextComp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _picture;

        public void SetText(TextContainerSO cont)
        {
            _title.text = cont.Title;
            _text.text = cont.GetFormattedText;
            _picture.sprite = cont.Picture;
        }


    }
}