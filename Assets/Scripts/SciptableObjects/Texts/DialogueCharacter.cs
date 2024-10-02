using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Texts
{
    [CreateAssetMenu(fileName = "New Dialogue Character", menuName = "Game/Dialogues/Dialogue Char")]
    public class DialogueCharacter : ScriptableObject
    {
        public string CharacterName;
        public SerializedDictionary<FaceExpression, Sprite> Pictures;
        
    }
}