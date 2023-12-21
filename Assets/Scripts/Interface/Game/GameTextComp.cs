using TMPro;
using UnityEngine;
namespace Arcatech
{
    public class GameTextComp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(TextContainerSO cont)
        {
            var txt = new TextContainer(cont);
            _title.text = txt.GetTitle;
            _text.text = txt.GetFormattedText;
        }


    }
}