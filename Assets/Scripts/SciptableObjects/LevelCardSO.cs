using UnityEngine;
[CreateAssetMenu(fileName = "New level card", menuName = "Configurations/Level Card")]
public class LevelCardSO : ScriptableObjectID
{
    public int SceneLoaderIndex;
    [Space]
    public string LevelNameShort;
    public string nextID;
    public string TextContainerID;
    public LevelType LevelType;
    public AudioClip Music;

}
