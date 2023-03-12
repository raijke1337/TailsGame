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
    protected bool _act;
    public bool IsNeeded
    {
        get => _act;
        set
        {
            _act = value;
            gameObject.SetActive(value);
        }
    }

}
