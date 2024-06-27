using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers.Save;
using Arcatech.Scenes;
using Arcatech.Units;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace Arcatech.Managers
{

    public class DataManager : MonoBehaviour
    {
        #region SingletonLogic

        public static DataManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _bind = new EventBinding<InventoryUpdateEvent>(OnInventoryUpdate);
                EventBus<InventoryUpdateEvent>.Register(_bind);


                LoadSave();
            }
            else Destroy(gameObject);
        }

        private void OnDisable()
        {
            EventBus<InventoryUpdateEvent>.Deregister(_bind);
        }
        #endregion
        #region saving

        private LoadedGameSave _loadedSave;
        public bool IsNewGame
        {
            get
            {
                if (_loadedSave == null)
                {
                    LoadSave();
                }    
                return _loadedSave.IsNewGame;
            }
        }
        public UnitInventoryItemConfigsContainer GetCurrentPlayerItems
        {
            get
            {
                if (_loadedSave == null)
                {
                    LoadSave();
                }
                return _loadedSave.CurrentInventory;
            }
        }
        public void UpdateCurrentPlayerItems (UnitInventoryComponent comp)
        {
            _loadedSave.IsNewGame = false;
            _loadedSave.UpdateCurrentInventory(comp);
        }

        EventBinding<InventoryUpdateEvent> _bind;

        private void LoadSave()
        {
            _loadedSave = new LoadedGameSave();
            if (_loadedSave.IsNewGame)
            {
                Debug.Log($"No save found, starting new game");
                CreateDefaultSave();
            }
        }

        public class LoadedGameSave
        {
            public List<string> OpenedLevelsID;
            public UnitInventoryItemConfigsContainer CurrentInventory { get; set; }
            private SavesSerializer _ser;
            public bool IsNewGame { get; set; }

            public LoadedGameSave()
            {
                _ser = new SavesSerializer();
                OpenedLevelsID = new List<string>();
                if (_ser.TryLoadSerializedSave(out var loaded))
                {
                    OpenedLevelsID.AddRange(loaded.OpenedLevelIDs);
                    CurrentInventory = new UnitInventoryItemConfigsContainer(loaded.PlayerItems);

                    IsNewGame = false;
                }
                else
                {
                    CurrentInventory = new UnitInventoryItemConfigsContainer();
                    IsNewGame = true;
                }
            }
            public void PackSerializedSaveData()
            {
                _ser.UpdateSerializedSave(this);
            }
            public void UpdateCurrentInventory(UnitInventoryComponent comp)
            {
                CurrentInventory = new UnitInventoryItemConfigsContainer(comp);
            }
        }
        private void CreateDefaultSave()
        {
            RefreshSceneContainers();

            _loadedSave.CurrentInventory = new UnitInventoryItemConfigsContainer();
            List<SceneContainer> list = new List<SceneContainer>();
            foreach (var cont in _allLevels)
            {
                if (cont.IsUnlockedByDefault && cont.LevelType == LevelType.Game) list.Add(cont);
            }
            _loadedSave.PackSerializedSaveData();

        }

        public void AddUnlockedLevelToSave(SceneContainer lv, bool writeSave = false)
        {
            _loadedSave.IsNewGame = false;
            if (!_loadedSave.OpenedLevelsID.Contains(lv.ID))
            {
                _loadedSave.OpenedLevelsID.Add(lv.ID);
            }
            if (writeSave) _loadedSave.PackSerializedSaveData();
        }

        public void OnNewGame()
        {
            if (_loadedSave == null) LoadSave();
        }


        private void OnApplicationQuit()
        {
            _loadedSave.PackSerializedSaveData();
        }

        #endregion

        #region level containers

        private List<SceneContainer> _allLevels;
        public List<SceneContainer> GetAvailableLevels
        { 
            get
            {
                if (_loadedSave == null)
                {
                    LoadSave();
                }


                List<SceneContainer> list = new List<SceneContainer>();
                foreach (string opened in _loadedSave.OpenedLevelsID)
                {
                    try 
                    {
                        list.Add(_allLevels.First(t => t.ID == opened));
                    }
                    catch
                    {
                        Debug.LogError($"No scene container for ID {opened}");
                    }
                }
                return list;
            }
        }

        private void OnInventoryUpdate(InventoryUpdateEvent arg)
        {
            if (arg.Unit is PlayerUnit)
            _loadedSave.UpdateCurrentInventory(arg.Inventory);
        }


        private void RefreshSceneContainers()
        {
            if (_allLevels == null)
            {
                _allLevels = new List<SceneContainer>();
                foreach (var l in GetAssetsOfType<SceneContainer>(Constants.Configs.c_LevelsPath))
                {
                    _allLevels.Add(l);
                }
            }
        }
        public SceneContainer GetSceneContainer(string ID)
        {
            RefreshSceneContainers();
            try
            {
                return _allLevels.First(t => t.ID == ID);
            }
            catch
            {
                Debug.LogWarning($"Scene container with ID {ID} not found");
                return null;
            }
        }
        public SceneContainer GetSceneContainer(int index)
        {
            RefreshSceneContainers();
            try
            {
                return _allLevels.First(t => t.SceneLoaderIndex == index);
            }
            catch
            {
                Debug.LogWarning($"Scene container with loader index {index} not found");
                return null;
            }
        }

        #endregion



        // only us3ed for items creation from cfgs for now
        #region configs
        [SerializeField] private List<ScriptableObjectID> _dictSO;
        private void FindAllConfigs()
        {

            _dictSO = new List<ScriptableObjectID>();
            //Resources.FindObjectsOfTypeAll<ScriptableObjectID>());


            string path = Constants.Configs.c_AllConfigsPath;
            string appPath = Application.dataPath;
            List<string> files = new List<string>();

            Stack<string> workPaths = new Stack<string>(new string[1] { path });
            while (workPaths.Count > 0)
            {
                string currFolder = workPaths.Pop();
                string[] foundSubfolders = Directory.GetDirectories(appPath + currFolder);
                string[] foundFiles = Directory.GetFiles(appPath + currFolder);
                files.AddRange(foundFiles);

                foreach (string foundpath in foundSubfolders)
                {
                    int index = foundpath.IndexOf(path);
                    var foldername = foundpath.Substring(index);
                    workPaths.Push(foldername + "/");
                }
            }

            foreach (string found in files)
            {
                var foundRelativ = found.Replace(appPath.ToString(), "Assets");

                var file = AssetDatabase.LoadAssetAtPath<ScriptableObjectID>(foundRelativ);
                if (file is ScriptableObjectID)
                {
                    _dictSO.Add(file);
                }
            }
            //Debug.Log($"{this} loaded total {_dictSO.Count} config objects");

        }


        /// <summary>
        /// get configs of T type
        /// </summary>
        /// <typeparam name="T">config SO</typeparam>
        /// <param name="path">refer to Constants</param>
        /// <param name="includeSubDirs">look in subfolders </param>
        /// <returns>list of assets in specified folder</returns>
        public T GetConfigByID<T>(string ID = "default") where T : ScriptableObjectID
        {
            List<T> list = new List<T>();

            //Debug.Log($"Looking for cfg {ID} type {typeof(T)}");

            if (_dictSO.Count == 0)
            {
                FindAllConfigs();
            }

            foreach (var item in _dictSO)
            {
                if (item is T)
                {
                    list.Add(item as T);
                }

            }
            try
            {
                var result = list.First(t => t.ID == ID);
                return result;
            }
            catch
            {
                Debug.LogWarning($"Couldnt find {typeof(T)} with ID {ID}");
                return null;
            }
        }

        public T[] GetAssetsOfType<T>(string path) where T : class
        {
            string appPath = Application.dataPath;

            List<T> all = new List<T>();

            List<string> files = new List<string>();

            Stack<string> workPaths = new Stack<string>(new string[1] { path });
            while (workPaths.Count > 0)
            {
                string currFolder = workPaths.Pop();
                string[] foundSubfolders = Directory.GetDirectories(appPath + currFolder);
                string[] foundFiles = Directory.GetFiles(appPath + currFolder);
                files.AddRange(foundFiles);

                foreach (string foundpath in foundSubfolders)
                {
                    int index = foundpath.IndexOf(path);
                    var foldername = foundpath.Substring(index);
                    workPaths.Push(foldername + "/");
                }
            }

            foreach (string found in files)
            {
                var foundRelativ = found.Replace(appPath.ToString(), "Assets");

                var file = AssetDatabase.LoadAssetAtPath(foundRelativ, typeof(T));
                if (file is T)
                {
                    all.Add(file as T);
                }
            }
            return all.ToArray();
        }
        //public UnitInventoryItemConfigsContainer GenerateDefaultInventory(UnitItemsSO cfg)
        //{
        //    return new UnitInventoryItemConfigsContainer(cfg);
        //}

        #endregion

        #region prefs
        public void SetPlayerPref(string pref, object value)
        {
            if (value is float)
            {
                PlayerPrefs.SetFloat(pref, (float)value);
            }
            if (value is int)
            {
                PlayerPrefs.SetInt(pref, (int)value);
            }
            if (value is string)
            {
                PlayerPrefs.SetString(pref, (string)value);
            }
        }

        #endregion
    }
}