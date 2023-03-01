using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region SingletonLogic

    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public void PlaySound(string ID)
    {
        Debug.Log($"{this} requested to play sound {ID}");
    }

}
