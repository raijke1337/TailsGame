using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Texts
{
    [CreateAssetMenu(fileName = "New Dialogue part part", menuName = "Game Stuff/Dialogues/Dialogue")]
    public class DialoguePart : ScriptableObject
    {
        public DialogueCharacter Character;

        public FaceExpression Mood;
        public SimpleText DialogueContent;

        public SerializedDictionary<SimpleText, DialoguePart> Options;

    }
}