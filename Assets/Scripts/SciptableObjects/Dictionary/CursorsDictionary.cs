using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Cursors Dict", menuName = "Dictionary/Cursors", order = 1)]
public class CursorsDictionary : ScriptableObjectID
{
    [SerializeField] SerializableDictionaryBase<CursorType, Texture2D> _dict;
    public Dictionary<CursorType, Texture2D> GetCursors { get
        {
            var d = new Dictionary<CursorType, Texture2D>();
            foreach (var key in _dict.Keys)
            {
                d[key] = _dict[key];
            }
            return d;
        }
    }

}

