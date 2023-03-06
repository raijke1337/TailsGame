//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public GameMode GameMode { get; private set; }

    [SerializeField] private LoadedManagers GameControllersPrefab;
    [SerializeField] private GameInterfaceManager gameInterfaceManagerPrefab;
    private LoadedManagers _gameControllers;

    public LoadedManagers GetGameControllers => _gameControllers;
    public GameInterfaceManager GetGameInterfacePrefab => gameInterfaceManagerPrefab;



    public LevelData GetLevelData(string ID) => _levels[ID];
    private Dictionary<string, LevelData> _levels;

    private LevelData _currentLevel;


    public LevelData GetCurrentLevelData { get => _currentLevel; }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        var levelCards = DataManager.Instance.GetAssetsOfType<LevelCardSO>(Constants.Configs.c_LevelsPath);
        _levels = new Dictionary<string, LevelData>();
        foreach (var level in levelCards)
        {
            _levels[level.ID] = new LevelData(level);
        }
    }

    private void Update()
    {
        if (_currentLevel != null && _currentLevel.Type == LevelType.Game && _gameControllers != null)
        {
            _gameControllers.UpdateManagers(Time.deltaTime);
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        switch (_currentLevel.Type)
        {
            case LevelType.Menu:
                break;
            case LevelType.Game:
                _gameControllers = Instantiate(GameControllersPrefab);
                _gameControllers.Initiate(_currentLevel);
                break;
            case LevelType.Scene:
                _gameControllers = Instantiate(GameControllersPrefab);
                _gameControllers.Initiate(_currentLevel);
                break;
        }
    }

    public void RequestLevelLoad(string ID)
    {
        LoadLevel(ID);
    }

    public void OnFinishedEquips()
    {
        LoadLevel(gameLevelID,true);
    }

    private string gameLevelID;
    private void LoadLevel(string ID, bool gameOverride = false)
    {
        try
        {
            _currentLevel = _levels[ID];
            if (_currentLevel.Type != LevelType.Game)
            {
                SceneManager.LoadScene(_currentLevel.SceneLoaderIndex);
            }
            if (_currentLevel.Type == LevelType.Game && gameOverride)
            {
                SceneManager.LoadScene(_levels[ID].SceneLoaderIndex);
            }
            if (_currentLevel.Type == LevelType.Game && !gameOverride)
            {
                gameLevelID = ID;
                LoadLevel("equips");
            }
        }
        catch
        {
            Debug.Log($"No {ID} in level cards");
        }
    }

    public void OnStartNewGame()
    {
        DataManager.Instance.CreateDefaultSave();
        RequestLevelLoad("intro");
    }


    #region game events

    public void OnLevelComplete()
    {
        if (!DataManager.Instance.GetSaveData.OpenedLevels.Contains(_currentLevel.LevelID))
        {
            DataManager.Instance.GetSaveData.OpenedLevels.Add(_currentLevel.LevelID);
        }
        DataManager.Instance.UpdateSaveData();
        Debug.Log("Level complete");

    }


    public void OnPlayerDead()
    {
        Debug.LogWarning("You died");
        // RequestLevelLoad(0);
    }
    public void OnItemPickup(string itemID)
    {
        DataManager.Instance.GetSaveData.PlayerItems.InventoryIDs.Add(itemID);
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#endif
    }

    #endregion

    #region SingletonLogic

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    #endregion
}


