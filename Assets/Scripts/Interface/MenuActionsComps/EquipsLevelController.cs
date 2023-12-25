using Arcatech.Items;
using Arcatech.UI;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Managers
{
    public class EquipsLevelController : MonoBehaviour
    {
        //private SerializedSaveData saveData;

        [SerializeField] private InventoryItemsHolder _items;
        [SerializeField] private EquipsPanel _equips;
        [SerializeField] private GameObject _weaponsMessage;

        [SerializeField] private Canvas _tooltips;
        [SerializeField] private TooltipComp _tooltipPrefab;
        private TooltipComp instantiatedTT;

        [SerializeField] private bool _showingTooltip = false;

        private PlayerUnit _player;


        public void OnDone()
        {
            if (_player.IsArmed)
            {
                GameManager.Instance.OnFinishedEquips();
            }
            else
            {
                _weaponsMessage.SetActive(true);
            }

        }
        public void OnMain()
        {
            GameManager.Instance.OnReturnToMain();
        }


        private void OnEnable()
        {
            _player = FindObjectOfType<PlayerUnit>();
            if (_player == null)
            {
                Debug.LogError($"Player unit not found!");
            }
            _player.InitiateUnit();

            if (_items == null | _equips == null)
            {
                Debug.LogError("Set panels for " + gameObject.name);
                return;
            }
            //saveData = DataManager.Instance.GetSaveData;
            // load items from player instaed
            // 
            _items.StartController();
            _equips.StartController();

            foreach (var i in _player.GetUnitInventory.GetCurrentEquips)
            {
                var tile = _equips.AddTileContent(i);
                tile.ItemClickedEvent += OnEquipmentTileClicked;
            }
            foreach (var e in _player.GetUnitInventory.GetCurrentInventory)
            {
                var tile = _items.AddTileContent(e);
                tile.ItemClickedEvent += OnInventoryTileClicked;
            }


        }

        private void OnInventoryTileClicked(InventoryItem arg)
        {

            if (arg is not EquipmentItem) return; // unequippable items


            ItemTileComponent tile = _items.RemoveTileContent(arg);
            tile.ItemClickedEvent -= OnInventoryTileClicked;


            // this just swaps subs for swithcing between windows
            var equipTile = _equips.AddTileContent(tile.Item);
            equipTile.ItemClickedEvent += OnEquipmentTileClicked;
            // make the 


            // TODO null here for some reason

            _player.GetUnitInventory.RemoveItem(arg);

            _player.GetUnitInventory.EquipItem(arg as EquipmentItem);
        }

        private void OnEquipmentTileClicked(InventoryItem arg)
        {
            // Debug.Log("Clicked callback - equipment");
            _equips.RemoveTileContent(arg).ItemClickedEvent -= OnEquipmentTileClicked;
            _items.AddTileContent(arg).ItemClickedEvent += OnInventoryTileClicked;

            _player.GetUnitInventory.MoveItemToInventory(_player.GetUnitInventory.UnequipItem(arg.ItemType));
        }

        private void Update()
        {
            _items.UpdateController(Time.deltaTime); // nothing here for now
            _equips.UpdateController(Time.deltaTime);

            //if (instantiatedTT == null)
            //{
            //    instantiatedTT = Instantiate(_tooltipPrefab);
            //    var rect = instantiatedTT.GetComponent<RectTransform>();
            //    rect.SetParent(_tooltips.transform);
            //}
            //instantiatedTT.gameObject.SetActive(_showingTooltip);
            //if (_showingTooltip)
            //{
            //    instantiatedTT.GetRect.anchoredPosition = Mouse.current.position.ReadValue();
            //}
        }

        private void HandleTooltipShow(InventoryItem arg1, bool arg2)
        {
            // _showingTooltip = arg2;
            //instantiatedTT.SetTexts(arg1);
        }

        private void OnDisable()
        {
            _items.StopController();
            _equips.StopController();
        }



    }
}