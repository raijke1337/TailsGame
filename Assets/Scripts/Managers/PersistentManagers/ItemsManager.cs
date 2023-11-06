using Arcatech.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Managers
{

    public class ItemsManager : MonoBehaviour
    {
        #region SingletonLogic

        public static ItemsManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
        }
        #endregion

        private DataManager _configs;
        private Dictionary<string, Item> _cachedItems;

        private void Start()
        {
            _configs = DataManager.Instance;
            if (_cachedItems == null)
            {
                _cachedItems = new Dictionary<string, Item>();
            }
        }


        public T GetNewItemByID<T>(string id) where T : InventoryItem
        {
            if (_cachedItems == null)
            {
                _cachedItems = new Dictionary<string, Item>();
            }

            if (!_cachedItems.ContainsKey(id))
            {
                var cfg = _configs.GetConfigByID<Item>(id);
                _cachedItems[id] = cfg;
            }

            var item = new InventoryItem(_cachedItems[id]);
            return item as T;

        }
    }
}