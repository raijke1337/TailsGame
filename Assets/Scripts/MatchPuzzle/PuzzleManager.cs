using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public delegate void MiniGameEvents<T>(T arg);
        public event SimpleEventsHandler<GameResult> GameCompletedEvent;

        [SerializeField] private RectTransform _field;
        [SerializeField, Space] private Text _results;

        [SerializeField] private TileComponent _tilePrefab;
        [SerializeField] private Sprite _closed;
        [SerializeField] private List<Sprite> _open;
        [SerializeField, Tooltip("Time the opened tiles are shown")] private float _graceTimer = 1f;
        [SerializeField, Tooltip("Total pairs to show")] private int _pairsTotal = 7;

        [SerializeField, Space] private Vector2 _tileOffsets;
        [SerializeField, Space] private Vector2 _desiredTileSize;

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
            var points = Extensions.GetTilePositions(_field, _tileOffsets, _desiredTileSize);
            for (int i = 0; i < _pairsTotal * 2; i++)
            {

                var coord = points[i];

                var item = Instantiate(_tilePrefab); _tiles.Add(item);
                item.SetImages(false, _closed);

                var rect = item.GetComponent<RectTransform>();
                item.transform.SetParent(_field, false);

                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _desiredTileSize.x);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _desiredTileSize.y);
                rect.anchoredPosition += coord;

                item.OnFlipEvent += Item_OnFlipEvent;
                item.name = $"Tile {i}";

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