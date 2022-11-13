using System.Linq;
using UnityEngine;

public class LevelSelectMenuComp : MenuPanel
{
    [SerializeField] RectTransform _content;
    private LevelsLoaderManager _levels;
    private GameManager _game;
    [SerializeField] private LevelButtonComp _buttonPrefab;

    private LevelButtonComp[] _buttons;

    private void OnEnable()
    {
        _levels = LevelsLoaderManager.GetInstance;
        _game = GameManager.GetInstance;
        if (_buttonPrefab == null) Debug.LogError("Set button prefab in " + this);
    }

    protected override void OnStateChange(bool isShow)
    {
        if (isShow)
        {
            CreateButtons();
        }
        else
        {
            foreach (var b in _buttons)
            {
                Destroy(b.gameObject);
            }
        }
    }
    public void CreateButtons()
    {
        int lastindex = _game.GetSaveData.LastLevelIndex;
        LevelData[] availableLevels = _levels.GetLevelData.Where(t => t.SceneLoaderIndex <= lastindex).ToArray();
        float vertOffs = _buttonPrefab.GetSize.y;

        _buttons = new LevelButtonComp[availableLevels.Length];
        for (int i = 0; i < availableLevels.Length; i++)
        {
            var b = Instantiate(_buttonPrefab);
            var rect = b.GetComponent<RectTransform>();
            rect.SetParent(_content,false);
            rect.localPosition = Vector3.zero;
            rect.position += new Vector3(0, -vertOffs*i, 0);
            b.LevelData = availableLevels[i];
            b.OnButtonClick += OnLevelSelected;

            _buttons[i] = b;
        }
    }

    private void OnLevelSelected(LevelButtonComp data)
    {
        _levels.RequestLevelLoad(data.LevelData.SceneLoaderIndex);
    }


}

