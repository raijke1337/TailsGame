using Arcatech.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Arcatech.Puzzles
{
    public class MatchTwoPuzzle : BasePuzzleComponent
    {

        [SerializeField] protected IconTileComp _tilePrefab;
        [SerializeField] protected Match2Settings _settings;
        [SerializeField] protected TextMeshProUGUI _timerText;
        [SerializeField] protected Slider _fillSlider;

        protected Dictionary<Pair<IconTileComp>, Sprite> _pairs;
        protected float _time;

        protected override void OnEnable()
        {base.OnEnable();
#if UNITY_EDITOR
            Assert.IsNotNull(_tilePrefab);
            Assert.IsNotNull(_settings);
            Assert.IsNotNull(_fillSlider);
#endif           

        }

        protected override void SetUpPuzzle()
        {
            _time = _settings.TimeToSolve;
            _fillSlider.maxValue = _time;
            _fillSlider.value = _time;

            _pairs = new Dictionary<Pair<IconTileComp>, Sprite>();

            List<IconTileComp> _unassignedTiles = new List<IconTileComp>();
            List<Sprite> _sprites = new List<Sprite> (GameUIManager.Instance.GetMatch2Sprites);

            int total = _settings.Pairs * 2;
            for (int i = 0; i <total; i++)
            {
                _unassignedTiles.Add(Instantiate(_tilePrefab,_gamePanel));
            }

            for (int x = 1; x <= _settings.Pairs; x++)
            {
                IconTileComp random1 = _unassignedTiles[Random.Range(0, _unassignedTiles.Count - 1)];
                _unassignedTiles.Remove(random1);
                IconTileComp random2 = _unassignedTiles[Random.Range(0, _unassignedTiles.Count - 1)];
                _unassignedTiles.Remove(random2);
                Sprite randomPic = _sprites[Random.Range(0, _sprites.Count - 1)];
                _sprites.Remove(randomPic);

                _pairs[new Pair<IconTileComp>(random1, random2)] = randomPic;

                random1.ClickedEvent += TileClicked;
            }
        }

        protected IconTileComp _selectedTile;
        protected void TileClicked(IconTileComp c)
        {
            var pair = _pairs.Keys.First(t => t.Contains(c));
            c.SetSprite(_pairs[pair]);

            if (_selectedTile == null)
            {
                _selectedTile = c;
            }
            else
            {
                bool match = false;
                foreach (var key in _pairs.Keys)
                {
                    if (key.Contains(c))
                    {
                        if (key.Matching(c, _selectedTile)) match = true;
                    }
                }
                if (!match)
                {
                    _selectedTile.Clear();
                    c.Clear();
                    _selectedTile = null;
                }
                else
                {
                    _selectedTile.ClickedEvent -= TileClicked;
                    c.ClickedEvent -= TileClicked;
                    _pairs.Remove(pair);
                    _selectedTile = null;
                    if (_pairs.Count == 0)
                    {
                        ResultCallback(true);
                    }
                }
            }
        }

        private void Update()
        {
            _time -= Time.deltaTime;
            _timerText.text = Mathf.RoundToInt(_time).ToString();
            _fillSlider.value = _time;

            if (_time <= 0) ResultCallback(false);
        }


    }
}