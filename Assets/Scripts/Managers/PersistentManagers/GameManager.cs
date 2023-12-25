//using Newtonsoft.Json;
using Arcatech.Items;
using Arcatech.Scenes;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcatech.Managers
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private PlayerUnit PlayerPrefab;
        [SerializeField,Tooltip("Level to use for equips change,started if requested to load a level of type GAME")] private SceneContainer EquipmentsScene;


        [Space,SerializeField] private LoadedManagers GameControllersPrefab;
        [SerializeField] private GameInterfaceManager gameInterfaceManagerPrefab;
        [SerializeField] private IsoCameraController gameCameraPrefab;

       
        private LoadedManagers _gameControllers;

        public LoadedManagers GetGameControllers => _gameControllers;
        public GameInterfaceManager GetGameInterfacePrefab => gameInterfaceManagerPrefab;
        public IsoCameraController GetGameCameraPrefab => gameCameraPrefab;

        #region levels management


        public SceneContainer[] GetDefaultGameLevels
        {
            get
            {
                return _levels.Values.Where(t => t.IsUnlockedByDefault == true && t.LevelType == LevelType.Game).ToArray();
            }
        }

        public SceneContainer GetCurrentLevelData { get => _currentLevel; }
        private SceneContainer _currentLevel;
        public SceneContainer GetLevelData(int index) => _levels[index];
        private Dictionary<int, SceneContainer> _levels;










        #endregion




        #region default
        private void OnEnable()
        {
            SceneManager.sceneLoaded += SwitchedScenesCleanUp;
        }

        private void Update()
        {
            if (_currentLevel != null && _currentLevel.LevelType == LevelType.Game && _gameControllers != null)
            {
                _gameControllers.UpdateManagers(Time.deltaTime);
            }
        }
        #endregion
        public void RequestLevelLoad(string ID)
        {
            LoadLevel(ID);
        }

        public void OnFinishedEquips()
        {
            var newequips = _gameControllers.UnitsManager.GetPlayerUnit.GetUnitInventory.PackSaveData;
            DataManager.Instance.UpdateSaveData(newequips);
            LoadLevel(gameLevelID, true);
        }

        private void LoadLevel(string ID, bool forceLoad = false)
        {
            EffectsManager.Instance.CleanUpOnSceneChange();
            if (ID == "") { LoadLevel("main", true); } // it happens in debug on level complete - the card has no "next level" ID
            if (forceLoad) // used in equips level 
            {
                _currentLevel = _levels[ID];
                SceneManager.LoadScene(_currentLevel.SceneLoaderIndex);
            }
            else
            {
                try
                {
                    var typeLoad = _levels[ID].LevelType;
                    switch (typeLoad)
                    {
                        case LevelType.Menu:
                            LoadLevel("equips", true);
                            break;
                        case LevelType.Scene:
                            LoadLevel(ID, true);
                            break;
                        case LevelType.Game:
                            gameLevelID = ID;
                            LoadLevel("equips", true);
                            break;
                    }
                }
                catch
                {
                    Debug.Log($"Something went wrong when switching to level {ID} from {_currentLevel.ID}");
                }
            }
        }

        private void SwitchedScenesCleanUp(Scene arg0, LoadSceneMode arg1)
        {
            _gameControllers = Instantiate(GameControllersPrefab);
            _gameControllers.Initiate(_currentLevel);
        }

        public void OnStartNewGame()
        {
            DataManager.Instance.OnNewGame();
            RequestLevelLoad("intro0");
        }
        public void OnReturnToMain()
        {
            LoadLevel("main", true);
        }


        #region game events
        public void OnPlayerPaused(bool isPausing)
        {
            _gameControllers.UnitsManager.GameplayPaused = isPausing; // shoukld stop player inputs
        }


        public void OnLevelCompleteTrigger(SceneContainer completedLV)
        {
            gameLevelID = completedLV.ID;
            OnFinishedEquips(); // update save data with picked up items
            if (_gameControllers != null)
            {
                _gameControllers.Stop();
            }

            var next = completedLV.NextLevel;

            DataManager.Instance.UpdateSaveData(completedLV);


            LoadLevel(next.ID);
        }


        public void OnPlayerDead()
        {
            _gameControllers.GameInterfaceManager.GameOver();
        }

        public void OnItemPickup(Item item)
        {
            _gameControllers.UnitsManager.GetPlayerUnit.GetUnitInventory.AddItem(item);
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


}