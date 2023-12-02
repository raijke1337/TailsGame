using Arcatech.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.UI
{
    public class InventoryItemsHolder : ManagedControllerBase
    {
        [SerializeField] protected ItemTileComponent TilePrefab;
        [SerializeField] protected Transform _iconsParent;
        protected List<ItemTileComponent> _tiles;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns>tile where the item is placed</returns>
        public virtual ItemTileComponent AddTileContent(InventoryItem content)
        {
            if (content == null) return null;
            if (_tiles == null) _tiles = new List<ItemTileComponent>();

            var tile = Instantiate(TilePrefab, _iconsParent);
            _tiles.Add(tile);

            tile.Item = content;

            tile.enabled = true;

            return tile;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns>tile that was destroyed</returns>
        public virtual ItemTileComponent RemoveTileContent(InventoryItem content)
        {
            if (content == null) return null;
            var tile = _tiles.First(t=>t.Item == content);
            _tiles.Remove(tile);
            Destroy(tile.gameObject);
            return tile;
        }


        public override void StartController()
        {
            _tiles = new List<ItemTileComponent>();
        }

        public override void UpdateController(float delta)
        {
            
        }

        public override void StopController()
        {
        }
    }
}