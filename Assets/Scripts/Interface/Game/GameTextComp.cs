using TMPro;
using UnityEngine;

public class GameTextComp : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public void SetText(TextContainer textc)
    {
        if (_text == null)
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        _text.text = textc.ToString();
    }

    public bool IsShown
    {
        get => gameObject.activeSelf;
        set
        {
            gameObject.SetActive(value);
        }
    }
}
