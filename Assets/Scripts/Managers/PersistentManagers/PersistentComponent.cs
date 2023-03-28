using UnityEngine;

public class PersistentComponent : MonoBehaviour
{
    public static PersistentComponent Comp;


    public bool SaveObject;
    private void Awake()
    {
        if (SaveObject) DontDestroyOnLoad(gameObject);
        if (Comp == null) Comp = this;
        else Destroy(gameObject);
    }
}
