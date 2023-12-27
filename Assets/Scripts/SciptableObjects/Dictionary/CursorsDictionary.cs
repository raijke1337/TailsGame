using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Cursors Dict", menuName = "Dictionary/Cursors", order = 1)]
public class CursorsDictionary : ScriptableObjectID
{
    [SerializeField] public SerializedDictionary<CursorType, Texture2D> Cursors;

}

