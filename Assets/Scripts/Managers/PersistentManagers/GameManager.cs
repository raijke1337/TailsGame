//using Newtonsoft.Json;
using Arcatech.Items;
using Arcatech.Scenes;
using Arcatech.UI;
using Arcatech.Units;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Arcatech.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region controllers

        #region SingletonLogic

        public static GameManager Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        #endregion
        [SerializeField] private PlayerUnit PlayerPrefab;
        [SerializeField] private MenuPrefabControllerComp _mainMenuPrefab;

        [Space,SerializeField] private LoadedManagers GameControllersPrefab;
        [SerializeField] private GameInterfaceManager gameInterfaceManagerPrefab;
        [SerializeField] private IsoCameraController gameCameraPrefab;

       
        private LoadedManagers _gameControllers;

        public LoadedManagers GetGameControllers => _gameControllers;
        public GameInterfaceManager GetGameInterfacePrefab => gameInterfaceManagerPrefab;
        public IsoCameraController GetGameCameraPrefab => gameCameraPrefab;


        private void OnEnable()
        {
            _dataManager = DataManager.Instance;
            SceneManager.sceneLoaded += SwitchedScenesCleanUp;
#if UNITY_EDITOR
            Assert.IsNotNull(_mainScene);
            Assert.IsNotNull(_equipsScene);
            Assert.IsNotNull(_galleryScene);
            Assert.IsNotNull(_newgameScene);
            Assert.IsNotNull(_noEquipsLevel);
            Assert.IsNotNull(_mainMenuPrefab);

            // for quicker load into debug level 

#endif
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            _currentLevel = _dataManager.GetSceneContainer(sceneIndex);


            if (_currentLevel == _mainScene)
            {
                Instantiate(_mainMenuPrefab);
            }
        }

        private void Update()
        {
            if (_currentLevel != null && (_currentLevel.LevelType == LevelType.Game || _currentLevel.LevelType == LevelType.Scene) && _gameControllers != null)
            {
                _gameControllers.UpdateManagers(Time.deltaTime);
            }
        }

        #endregion

        #region levels management

        private DataManager _dataManager;


        [Space(10f), SerializeField] private SceneContainer _mainScene;
        [SerializeField] private SceneContainer _galleryScene;
        [SerializeField] private SceneContainer _newgameScene;
        [SerializeField] private SceneContainer _noEquipsLevel; // tutorial
        [SerializeField] private SceneContainer _equipsScene;

        public SceneContainer GetCurrentLevelData { get => _currentLevel; }
        private SceneContainer _currentLevel;

        private bool _equipsDone = false;
        private SceneContainer _cachedGameLevel;
        public void RequestLoadSceneFromContainer(SceneContainer sc)
        {          
            switch (sc.LevelType)
            {
                default:
                    DoLoadScene(sc.SceneLoaderIndex);
                    break;
                case LevelType.Game:                  
                    if (sc == _noEquipsLevel)
                    {
                        _cachedGameLevel = sc;
                        DoLoadScene(_noEquipsLevel.SceneLoaderIndex);
                        break;
                    }// override default equips menu

                    if (_equipsDone)
                    {
                        _equipsDone = false;
                        DoLoadScene(_cachedGameLevel.SceneLoaderIndex);
                    }
                    else
                    {
                        _cachedGameLevel = sc;
                        DoLoadScene(_equipsScene.SceneLoaderIndex);
                    }

                    break;
            }            
        }

        private void DoLoadScene(int index)
        {
            Debug.Log($"Loading scene index {index} - {SceneManager.GetSceneByBuildIndex(index).name}\nCurrent level is: {SceneManager.GetActiveScene().name}");

            _currentLevel = _dataManager.GetSceneContainer(index);

            EffectsManager.Instance.CleanUpOnSceneChange();
            SceneManager.LoadScene(index);
        }

        #region UNITY UI
        public void OnStartNewGameButton()
        {
            DataManager.Instance.OnNewGame();
            RequestLoadSceneFromContainer(_newgameScene);
        }
        public void OnGalleryButton()
        {
            RequestLoadSceneFromContainer(_galleryScene);
        }
        public void OnExitButton()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
        #endregion

        public void OnFinishedEquips()
        {
            _equipsDone = true;
            _dataManager.UpdateInventoryInSave(_gameControllers.UnitsManager.GetPlayerUnit.GetUnitInventory);

            RequestLoadSceneFromContainer(_cachedGameLevel);
        }

        private void SwitchedScenesCleanUp(Scene arg0, LoadSceneMode arg1)
        {
            _gameControllers = Instantiate(GameControllersPrefab);
            _gameControllers.Initiate(_currentLevel);

            if (arg0.buildIndex == _mainScene.SceneLoaderIndex)
            {
                Instantiate(_mainMenuPrefab);
            }

        }




        #endregion


        #region game events
        public void OnPlayerPaused(bool isPausing)
        {
            //if (isPausing)
            //{
            //    Time.timeScale = 0;
            //}
            //else
            //{
            //    Time.timeScale = 1;
            //}
            _gameControllers.UnitsManager.GameplayPaused = isPausing; // shoukld stop player inputs
        }


        public void OnLevelCompleteTrigger(SceneContainer unlock)
        {
            _dataManager.UpdateInventoryInSave(_gameControllers.UnitsManager.GetPlayerUnit.GetUnitInventory); // update save data with picked up items

            if (_gameControllers != null)
            {
                _gameControllers.Stop();
            }

            if (unlock == null)
            {
                Debug.Log($"Level complete trigger has no next lv assigned, returning to main");
                OnReturnToMain();
            }
            else
            {
                _dataManager.AddUnlockedLevelToSave(unlock);
                RequestLoadSceneFromContainer(unlock);
            }


        }

        public void OnReturnToMain()
        {
            if (_gameControllers != null)
            {
                _gameControllers.Stop();
            }
            DoLoadScene(_mainScene.SceneLoaderIndex);
        }

        public void OnPlayerDead()
        {
            if (_gameControllers != null)
            {
                _gameControllers.Stop();
            }
            _gameControllers.GameInterfaceManager.GameOver();
        }
        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
        }

        #endregion


    }


}