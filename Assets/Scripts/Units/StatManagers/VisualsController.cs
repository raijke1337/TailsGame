using Arcatech.Items;
using Arcatech.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{
    public class VisualsController : MonoBehaviour
    {
        #region materials change

        [SerializeField] private List<Material> _materials;

        public int StagesTotal => _materials.Count;
        private SkinnedMeshRenderer _mesh;
        private int _matIndex = 0;
        public bool SetMaterialStage(int ind)
        {
            if (ind >= _materials.Count) return false;
            _matIndex = ind;
            UpdateMaterial();
            return true;
        }
        public bool AdvanceMaterialStage()
        {
            if (_matIndex == _materials.Count - 1) return false;
            _matIndex++;
            UpdateMaterial();
            return true;
        }

        private void UpdateMaterial()
        {
            //_mesh.material = _materials[_matIndex];
        }

        #endregion
        private void Start()
        {
            //_mesh = GetComponentsInChildren<SkinnedMeshRenderer>().First(t => t.name == "Model");
            UpdateMaterial();
            // GameManager.Instance.OnGameModeChanged += RemoveEffects;
        }

        //private void OnDisable()
        //{
        //    GameManager.GetInstance.OnGameModeChanged -= RemoveEffects;
        //}

        //private void RemoveEffects(GameMode mode)
        //{
        //    SetMaterialStage(0);
        //    if (_items.Count == 0) return;
        //    foreach (var i in _items.ToList())
        //    {
        //        Destroy(i.gameObject);
        //        _items.Remove(i);
        //    }
        //}

        #region equip items


        public ItemEmpties Empties { get => _empties; set => _empties = value; }
        private ItemEmpties _empties;
        private List<EquipmentItem> _list = new List<EquipmentItem>();

        //public void CreateVisualItem(string itemID)
        //{
        //    var item = new EquipmentItem(DataManager.Instance.GetConfigByID<Equip>(itemID));
        //    CreateVisualItem(item);
        //}
        public void CreateVisualItem(EquipmentItem item)
        {
            _list.Add(item);

            switch (item.ItemType)
            {
                default:
                    Debug.Log($"Equipping visual item {item.ID} and there is no logic for {item.ItemType} in {name}: NYI");
                    break;
                case (EquipItemType.MeleeWeap):
                    item.GetInstantiatedPrefab.transform.parent = _empties.MeleeWeaponEmpty;
                    break;
            }
        }
        //public void CreateVisualItem(string[] itemIDs)
        //{
        //    foreach (var i in itemIDs) CreateVisualItem(i);
        //}
        public void ClearVisualItems()
        {
            Debug.Log($"Clearing all visuals in {name} : NYI");
        }

        public void RemoveVisualItem(string itemID)
        {
            EquipmentItem item = _list.FirstOrDefault(t => t.ID == itemID);
            if (item != null)
            {
                switch (item.ItemType)
                {
                    default:
                        Debug.Log($"Unequipping visual item {item.ID} and there is no logic for {item.ItemType} in {name}: NYI");
                        break;

                }


                _list.Remove(item);
            }
        }
        #endregion
    }

}