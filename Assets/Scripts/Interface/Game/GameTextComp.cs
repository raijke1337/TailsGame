using TMPro;
using UnityEngine;
namespace Arcatech
{
    public class GameTextComp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(TextContainer cont)
        {
            _title.text = cont.GetTitle;
            _text.text = cont.GetFormattedText;
        }


    }
}