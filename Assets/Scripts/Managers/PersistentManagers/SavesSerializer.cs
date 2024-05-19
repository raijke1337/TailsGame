using Arcatech.Scenes;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using static Arcatech.Managers.DataManager;

namespace Arcatech.Managers.Save
{
    #region xml serialization


    internal class SavesSerializer
    {
        private SerializedSaveData _data;
        private string _path;

        public SavesSerializer()
        {
            _path =  Application.dataPath + Constants.Configs.c_SavesPath;
            _data = TryDeserializeData();
        }

        public bool TryLoadSerializedSave(out SerializedSaveData save)
        {
            save = _data;
            return _data != null;
        }

        public void UpdateSerializedSave (LoadedGameSave save)
        {
            _data = new SerializedSaveData(save);

            SaveDataXML(_data);
            Debug.Log($"Saved {_data}");
        }


        private void SaveDataXML(SerializedSaveData data)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SerializedSaveData));
            FileStream fs = new FileStream(_path, FileMode.Create);
            ser.Serialize(fs, data);
            fs.Close();
            AssetDatabase.Refresh();
        }
        private SerializedSaveData TryDeserializeData()
        {

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(SerializedSaveData));
                FileStream fs = new FileStream(_path, FileMode.Open);
                var data = (SerializedSaveData)ser.Deserialize(fs);
                fs.Close();

                AssetDatabase.Refresh();
                return data;
            }
            catch
            {
                Debug.Log("xml save data not found");
                return null;
            }
        }
    }
    [XmlRoot("GameSave"), Serializable]
    public class SerializedSaveData
    {
        public bool IsNewGame;

        public List<string> OpenedLevelIDs;
        public SerializedUnitInventory PlayerItems;

        public SerializedSaveData(LoadedGameSave save)
        {
            OpenedLevelIDs = new List<string>();
            foreach (var l in save.OpenedLevelsID)
            {
                OpenedLevelIDs.Add(l);
            }
            PlayerItems = new SerializedUnitInventory(save.CurrentInventory);
            IsNewGame = save.IsNewGame;
        }
        public SerializedSaveData()
        {
        }
    }
    [Serializable]
    public class SerializedUnitInventory
    {
        public List<string> Equips;
        public List<string> Inventory;
        public SerializedUnitInventory()
        {
            Equips = new List<string>();
            Inventory = new List<string>();
        }
        public SerializedUnitInventory(List<string> equips, List<string> inventory)
        {
            Equips = new List<string>();
            Inventory = new List<string>();
            Equips = equips;
            Inventory = inventory;
        }
        public SerializedUnitInventory(UnitInventoryItemConfigsContainer cfg)
        {
            Equips = new List<string>();
            Inventory = new List<string>();
            foreach (var e in cfg.Equipment)
            {
                Equips.Add(e.ID);
            }
            foreach (var i in cfg.Inventory)
            {
                Inventory.Add(i.ID);
            }

        }
    }
    #endregion
}