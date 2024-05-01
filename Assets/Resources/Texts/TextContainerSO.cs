using UnityEngine;
namespace Arcatech.Texts
{
    [CreateAssetMenu(fileName = "New Simple Description", menuName = "Description/Simple")]
    public class TextContainerSO : ScriptableObject
    {

        public string Title;
        public string Text;

    }

}