using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualsController : MonoBehaviour
{
    [SerializeField] private List<Material> _materials;
    public int StagesTotal => _materials.Count;
    private SkinnedMeshRenderer _mesh;
    private int _matIndex = 0;
    public ItemEmpties Empties { get => _empties; set => _empties = value; }
    private ItemEmpties _empties;
    private List<EquipmentBase> _items;
    #region materials
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
    public void AddItem(string itemID)
    {
        if (_items == null) _items = new List<EquipmentBase>();
        var item = ItemsManager.Instance.GetItemByID<EquipmentBase>(itemID);


        switch (item.GetContents.ItemType)
        {
            case EquipItemType.None:
                break;
            case EquipItemType.MeleeWeap:
                var inst = Instantiate(item, Empties.SheathedWeaponEmpty.position, Empties.SheathedWeaponEmpty.rotation, Empties.SheathedWeaponEmpty);
                _items.Add(inst);
                break;
            case EquipItemType.RangedWeap:
                inst = Instantiate(item, Empties.RangedWeaponEmpty.position, Empties.RangedWeaponEmpty.rotation, Empties.RangedWeaponEmpty);
                _items.Add(inst);
                break;
            case EquipItemType.Shield:
                break;
            case EquipItemType.Booster:
                break;
            case EquipItemType.Other:
                break;
        }
    }
    public void AddItem(string[] itemIDs)
    {
        foreach (var i in itemIDs) AddItem(i);
    }
    public void ClearVisualItems()
    {
        if (_items == null) return;
        foreach (var i in _items.ToList())
        {
            Destroy(i.gameObject);
            _items.Remove(i);
        }
    }

    #endregion

}

