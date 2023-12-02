using Arcatech;
using UnityEngine;

[CreateAssetMenu(menuName = "Text")]
public class TextContainerSO : ScriptableObject
{
    public string ID;
    public string Title;
    public string[] Texts;
}

