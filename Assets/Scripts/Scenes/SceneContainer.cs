using Arcatech.Effects;
using Arcatech.Texts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcatech.Scenes
{
    [CreateAssetMenu(fileName = "New level", menuName = "Game/Level")]
    public class SceneContainer : ScriptableObjectID
    {
        public int SceneLoaderIndex;
        public LevelType LevelType;
        public SimpleText Description;
        public SoundClipData Music;
        public bool IsUnlockedByDefault;
        public SceneContainer NextLevel;
    }
}