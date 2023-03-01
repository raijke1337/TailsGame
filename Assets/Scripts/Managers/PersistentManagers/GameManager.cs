//using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    //public GameMode GameMode { get; private set; }

    [SerializeField]
    private LoadedManagers GameControllersPrefab;
    private LoadedManagers _gameControllers;
    public LoadedManagers GetGameControllers => _gameControllers;


    #region levels

    public LevelData GetLevelData(string ID) => _levels.FirstOrDefault(t=>t.LevelID == ID);
    private LevelData[] _levels;
    private LevelData _currentLevel;
    private int _continueIndex;

    public LevelData GetCurrentLevelData { get => _currentLevel; }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        var levelCards = DataManager.Instance.GetAssetsOfType<LevelCardSO>(Constants.Configs.c_LevelsPath);
        _levels = new LevelData[levelCards.Length];
        for (int i = 0; i < levelCards.Length; i++)
        {
            _levels[i] = new LevelData(levelCards[i]);
        }
    }

    private void Update()
    {
        if (_currentLevel != null &&  _currentLevel.Type == LevelType.Game && _gameControllers != null)
        {
            _gameControllers.UpdateManagers(Time.deltaTime);
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        switch (_currentLevel.Type)
        {
            case LevelType.Menu:
                //GameMode = GameMode.Menus;
                break;
            case LevelType.Game:
                //GameMode = GameMode.Gameplay;
                _gameControllers = Instantiate(GameControllersPrefab);
                _gameControllers.Initiate(_currentLevel);
                break;
            case LevelType.Scene:
                _gameControllers = Instantiate(GameControllersPrefab);
                _gameControllers.Initiate(_currentLevel);
                break;
        }
    }

    public void OnLevelComplete()
    {
        DataManager.Instance.GetSaveData.OpenedLevels.Add(_currentLevel.LevelID);
        DataManager.Instance.UpdateSaveData();
        RequestLevelLoad(_currentLevel.NextLevelID);
    }
    private void RequestLevelLoad(string ID)
    {
        LevelData lv = _levels.First(t => t.LevelID == ID);
        _currentLevel = lv;
        if (lv.Type == LevelType.Menu && _gameControllers != null) { _gameControllers.Stop(); Destroy(_gameControllers); }

        SceneManager.LoadScene(lv.SceneLoaderIndex);
    }
    private void RequestLevelLoad(int index)
    {
        LevelData lv = _levels.First(t => t.SceneLoaderIndex == index);
        //case : this is called from level select menu (continue game) and we need the equips menu first
        switch (lv.Type)
        {
            case LevelType.Menu:
                _currentLevel = lv;
                SceneManager.LoadScene(lv.SceneLoaderIndex);
                break;
            case LevelType.Scene:
                break;
            case LevelType.Game:
                break;
        }

        
        if (lv.Type == LevelType.Menu && _gameControllers != null) { _gameControllers.Stop(); Destroy(_gameControllers); }

        
    }


    public void OnNewGame()
    {
        DataManager.Instance.CreateDefaultSave();
        OnContinueGame(1); // 3 is debug level 1 is equips
    }
    public void OnContinueGame(string id)
    {
        RequestLevelLoad(id);
    }  
    public void OnContinueGame(int index)
    {
        RequestLevelLoad(index);
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


