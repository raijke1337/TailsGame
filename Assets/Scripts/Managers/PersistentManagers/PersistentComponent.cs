using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentComponent : MonoBehaviour
{
    public bool SaveObject;
    private void Awake()
    {
        if (SaveObject) DontDestroyOnLoad(gameObject);
    }
}
