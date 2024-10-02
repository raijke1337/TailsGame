using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech
{
    [CreateAssetMenu(fileName = "New Cursors Dict", menuName = "Game/Cursors", order = 1)]
    public class CursorsDictionary : ScriptableObjectID
    {
        [SerializeField] public SerializedDictionary<CursorType, Texture2D> Cursors;

    }

}