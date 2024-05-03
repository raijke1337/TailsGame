using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcatech.Items;
using Arcatech.Units;
using UnityEngine.UI;
using UnityEngine.Assertions;
using AYellowpaper.SerializedCollections;
using System.Linq;
using TMPro;

namespace Arcatech.UI
{
    public class EquipmentsMenuContainerScript : MonoBehaviour
    {
        #region display

        private UnitInventoryComponent _playerInventory;

        [SerializeField] SerializedDictionary<EquipItemType, ItemTileComponent> _equipsTiles;
        [SerializeField] List<ItemTileComponent> _inventoryTiles = new List<ItemTileComponent>();

        [Space, SerializeField] private ItemTileComponent _tilePreset; // for items
        [SerializeField] private Transform _tileParent; // for items too


        public void InitialInventoryDisplay (UnitInventoryComponent inv)
        {
            _playerInventory = inv;
            UpdateInventoryDisplay();
            _itemTooltipPanel.gameObject.SetActive(false);
            _skillTooltipPanel.gameObject.SetActive(false);
            _swapItemsButton.gameObject.SetActive(false);
        }
              
        public void UpdateInventoryDisplay()
        {
            Assert.IsNotNull(_tilePreset);
            Assert.IsNotNull(_tileParent);
            Assert.IsNotNull(_equipsTiles);
            Assert.IsNotNull(_playerInventory);
            Assert.IsNotNull(_swapItemsButton);

            //clean up
            foreach (var tile in _equipsTiles.Values)
            {
                tile.Item = null;
                tile.IconClickedEvent -= IconTileClicked;
            }
            foreach (var tile in _inventoryTiles)
            {
                tile.Item = null;
                tile.IconClickedEvent -= IconTileClicked;
            }

            if (_hldTile != null)
            {
                _hldTile.HighLightToggle();
                _hldTile = null;
            }

            //draw
            foreach (EquipmentItem equip in _playerInventory.GetCurrentEquips)
            {
                _equipsTiles[equip.ItemType].Item = equip;
                _equipsTiles[equip.ItemType].IconClickedEvent += IconTileClicked;
            }
            var inve = _playerInventory.GetCurrentInventory;

            int placedTiles = _inventoryTiles.Count;

            int counter = 0;
            while (counter < placedTiles && counter < inve.Count)
            {
                _inventoryTiles[counter].gameObject.SetActive(true);
                _inventoryTiles[counter].IconClickedEvent += IconTileClicked;
                _inventoryTiles[counter].Item = inve.ToArray()[counter];
                counter++;
            }

            if (counter == placedTiles)
            // case : not enough tiles
            {
                while (counter < inve.Count)
                {
                    var tile = Instantiate(_tilePreset, _tileParent);
                    tile.Item = inve.ToArray()[counter];
                    _inventoryTiles.Add(tile);
                    tile.IconClickedEvent += IconTileClicked;
                    counter++;
                }
            }
            //case : some tiles left : deactivate the empty ones
            if (counter == inve.Count)
            {
                while (counter < _inventoryTiles.Count)
                {
                    _inventoryTiles[counter].gameObject.SetActive(false);
                    counter++;
                }
            }

        }




        #endregion


        #region un(equip)

        [SerializeField] private TextMeshProUGUI _swapItemsButton;
        private InventoryItem _selectedItem;
        public void UI_OnUpdateButtonClick()
        {
            _playerInventory.HandleSwapButton(_selectedItem);
            _itemTooltipPanel.gameObject.SetActive(false);
            _skillTooltipPanel.gameObject.SetActive(false);
            _swapItemsButton.gameObject.SetActive(false);

            UpdateInventoryDisplay();
        }


        #endregion

        #region click item

        private IconTileComp _hldTile;
        private void IconTileClicked(IconTileComp arg)
        {
            if (_hldTile != null) { _hldTile.HighLightToggle(); }

           // Debug.Log($"Registered click on {arg.Item.ID} in {arg.name}");
            arg.HighLightToggle();
            _hldTile = arg;
            ItemTileClicked(arg.Item);
        }
        private void ItemTileClicked(InventoryItem arg)
        {
            _selectedItem = arg;
            ShowTooltips(arg);
            ShowButton(arg);
        }

        [SerializeField] TooltipPanelComp _itemTooltipPanel;
        [SerializeField] TooltipPanelComp _skillTooltipPanel;

        private void ShowTooltips(InventoryItem item)
        {
            _itemTooltipPanel.gameObject.SetActive(false);
            _skillTooltipPanel.gameObject.SetActive(false);

            if (item == null) return;

            var desc = item.GetDescription;
            try
            {
                _itemTooltipPanel.gameObject.SetActive(true);
                _itemTooltipPanel.Texts = desc;
            }
            catch { }

            if (item is EquipmentItem e)
            {
                try
                {
                    _skillTooltipPanel.gameObject.SetActive(true);
                    desc = e.Skill.Description;

                    _skillTooltipPanel.Texts = desc;

                }
                catch { }
            }
        }

        private void ShowButton(InventoryItem item)
        {
            _swapItemsButton.gameObject.SetActive(false);
            if (item is EquipmentItem eq)
            {
                _swapItemsButton.gameObject.SetActive(true);
                if (_playerInventory.IsItemEquipped(eq))
                {
                    _swapItemsButton.SetText("UNEQUIP");
                }
                else
                {
                    _swapItemsButton.SetText("EQUIP");
                }
            }
        }

        #endregion





    }
}