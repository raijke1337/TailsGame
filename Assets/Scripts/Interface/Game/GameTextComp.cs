using Arcatech.Texts;
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
            if (cont == null) return;

            _title.text = cont.Title;
            _title.font = GameUIManager.Instance.GetFont(FontType.Title);
            _text.text = cont.Text;
            _text.font = GameUIManager.Instance.GetFont(FontType.Text);
           // _picture.sprite = cont.Picture;
        }


    }
}