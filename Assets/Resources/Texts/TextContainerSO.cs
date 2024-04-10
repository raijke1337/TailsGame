using UnityEngine;

[CreateAssetMenu(fileName = "New Description Cointainer", menuName = "Description Container")]
public class TextContainerSO : ScriptableObject
{
    public Sprite Picture;
    public string Title;
    public string Text;

    public string GetFormattedText
    {
        get
        {
            return Text;
        }
    }
}

