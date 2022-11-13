using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagersBootStrapper : MonoBehaviour
{
    public GameMode CurrentGameMode = GameMode.Menus;

    public GameManager GameManager;
    public TextsManager TextsManager;
    public LevelsLoaderManager LevelsLoaderManager;


    private void Awake()
    {
        if (GameManager.GetInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        if (GameManager.GetInstance == null) { Instantiate(GameManager, transform); GameManager.InitSingleton(); }
        if (TextsManager.GetInstance == null) { Instantiate(TextsManager, transform); TextsManager.InitSingleton(); }
        if (LevelsLoaderManager.GetInstance == null) { Instantiate(LevelsLoaderManager, transform); LevelsLoaderManager.InitSingleton(); }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {

    }
}
