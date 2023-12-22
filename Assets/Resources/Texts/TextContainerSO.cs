using UnityEngine;

[CreateAssetMenu(menuName = "Text")]
public class TextContainerSO : ScriptableObject
{
    public Sprite Picture;
    public string Title;
    public string[] Texts;

    public string GetFormattedText
    {
        get
        {
            string result = string.Empty;

            foreach (string rec in Texts)
            {
                result += $"{rec} \n";
            }

            return result;
        }
    }
}

