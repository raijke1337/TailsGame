using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Texts
{
    [CreateAssetMenu(fileName = "New Extended Description", menuName = "Description/Extended")]
    public class ExtendedTextContainerSO : TextContainerSO
    {
        public Sprite Picture;
        public string FlavorText;
    }
}