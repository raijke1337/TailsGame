using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
namespace Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public delegate void MiniGameEvents<T> (T arg);
        public event SimpleEventsHandler<GameResult> GameCompletedEvent;

        [SerializeField] private TileComponent _tilePrefab;
        [SerializeField] private Sprite _closed;
        [SerializeField] private List<Sprite> _open;
        //[SerializeField,Range(2,14),Tooltip("Rounded up to even number, 14 max because only 7 pics for now")] private int _totalTiles;
        [SerializeField, Tooltip("Time the opened tiles are shown")] private float _graceTimer = 1f;

        [SerializeField,Space] private int _tileHeight;
        [SerializeField] private int _tileWidth;

        // todo - setup by number of tiles desired, not by size

        [Space, SerializeField] private RectTransform _field;
        [SerializeField] private Text _results;

        private List<TileComponent> _tiles = new List<TileComponent>();
        private TileComponent[] _selected = new TileComponent[2]; 
        private int _flipped = 0;

        private bool isBusy;
        private float _time;
        private Coroutine _timer;
        #region setup

        public void OnStartGame()
        {
            _timer = StartCoroutine(ElapsedCor());
            PlaceTiles();
            _tiles = Extensions.ShuffledList(_tiles);
            AssignPairs();
        }
        private void PlaceTiles()
        {
            int fieldHeight = (int)_field.rect.height;
            int fieldWidth = (int)_field.rect.width;


            int tilesHor = fieldWidth / _tileWidth;
            int tilesVert = fieldHeight / _tileHeight;
            //Debug.Log($"Calculated tiles: horizontal {tilesHor} vertical {tilesVert}");
            //Debug.Log($"Field size: height {fieldHeight} width {fieldWidth}");


            int allTilesWidth = _tileWidth * tilesHor;
            int allTilesHeight = _tileHeight * tilesVert;

            int totalOffsetWidth = fieldWidth - allTilesWidth;
            int totalOffsetHeight = fieldHeight - allTilesHeight;

            int singleOffsetWidth = totalOffsetWidth / (tilesHor + 1);
            int singleOffsetHeight = -(totalOffsetHeight / (tilesVert + 1));

            Vector2 DefaultOffset = new Vector2(singleOffsetWidth, singleOffsetHeight);

            //Debug.Log($"Calculated offsets: Vert {singleOffsetHeight} Hor {singleOffsetWidth}");
            if (tilesHor * tilesVert > 14) Debug.LogError("Please set bigger tiles, number > 14 not implemented");

            for (int i = 0; i < tilesHor; i++)
            {
                for (int j = 0; j < tilesVert; j++)
                {
                    var item = Instantiate(_tilePrefab); _tiles.Add(item);
                    item.SetImages(false, _closed);

                    var rect = item.GetComponent<RectTransform>();
                    item.transform.SetParent(_field, false);

                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _tileWidth);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _tileHeight);

                    rect.anchoredPosition += DefaultOffset;
                    rect.anchoredPosition += new Vector2(i * (singleOffsetWidth + _tileWidth), j * (singleOffsetHeight - _tileHeight));
                    //Debug.Log($"Tile {i} {j} pos: {rect.anchoredPosition}");
                    item.OnFlipEvent += Item_OnFlipEvent;
                    item.name = $"Tile {i},{j}";
                }
            }
        }
        private void AssignPairs()
        {            
            int halfCount = _tiles.Count / 2;
            for (int i = 0; i < halfCount; i++)
            {
                TileComponent a = _tiles[i];
                TileComponent b = _tiles[i + halfCount];
                a.Pair = b; b.Pair = a;
                a.SetImages(true, _open[i]);
                b.SetImages(true, _open[i]);
                //Debug.Log($"Paired {a} and {b}");
            }

        }

        #endregion

        #region game

        private void Item_OnFlipEvent(TileComponent tile)
        {
            if (isBusy) return;

            if (!_tiles.Contains(tile)) return; // will remove opened tiles from list
            _selected[_flipped] = tile;
            tile.Flip();
            _flipped++;
            if (_flipped == 2)
            {
                if (_selected[0].Pair == _selected[1])
                {
                    _tiles.Remove(_selected[0]);
                    _tiles.Remove(_selected[1]);
                    _tiles.TrimExcess();
                    if (_tiles.Count == 0) OnGameComplete(new GameResult(true, _time));
                }
                else
                {
                    StartCoroutine(GraceTimerCor(_graceTimer, _selected[0], _selected[1]));
                }
                _flipped = 0;
            }
        }

        private void OnGameComplete(GameResult result)
        {
            StopCoroutine(_timer);
            foreach (var tile in _tiles) { tile.OnFlipEvent -= Item_OnFlipEvent; }
            _results.text = $"Game completed, time elapsed: {result.timeElapsed}";
            GameCompletedEvent?.Invoke(result); 
        }

        private IEnumerator ElapsedCor()
        {
            while (true)
            {
                _time += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator GraceTimerCor(float time, TileComponent a, TileComponent b)
        {
            isBusy = true;
            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            a.Flip();
            b.Flip();
            isBusy = false;
            yield return null;
        }
    }

    

    public struct GameResult
    {
        public bool isWin;
        public float timeElapsed;
        public GameResult(bool iswin, float time)
        {
            isWin = iswin; timeElapsed = time;
        }
    }
    #endregion
}