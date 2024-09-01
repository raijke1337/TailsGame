using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Texts
{

    [CreateAssetMenu(fileName = "New Simple Description", menuName = "Game Stuff/Description/Simple")]
    public class SimpleText : ScriptableObject
    {
        public string Title;
        public string Text;
    }
}