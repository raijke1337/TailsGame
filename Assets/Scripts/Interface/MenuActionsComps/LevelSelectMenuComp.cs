using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMenuComp : MenuPanel
{
    [SerializeField] RectTransform _content;

    [SerializeField] private LevelButtonComp _buttonPrefab;

    private LevelButtonComp[] _buttons;

    private void OnEnable()
    {
        if (_buttonPrefab == null) Debug.LogError("Set button prefab in " + this);
    }

    protected override void OnStateChange(bool isShow)
    {
        if (isShow)
        {
            CreateButtons();
        }
        if (_buttons != null && !isShow)
        {
            foreach (var b in _buttons)
            {
                Destroy(b.gameObject);
            }
        }
    }
    public void CreateButtons()
    {
        var save = DataManager.Instance.GetSaveData;
        List<LevelData> _lvls = new List<LevelData>();
        foreach (var l in save.OpenedLevels)
        {
            var lv = GameManager.Instance.GetLevelData(l);
            if (lv != null && lv.Type == LevelType.Game)
            {
                _lvls.Add(lv);
            }
        }
        float vertOffs = _buttonPrefab.GetSize.y;

        _buttons = new LevelButtonComp[_lvls.Count];
        for (int i = 0; i < _lvls.Count; i++)
        {
            var b = Instantiate(_buttonPrefab);
            var rect = b.GetComponent<RectTransform>();
            rect.SetParent(_content, false);
            rect.position += new Vector3(0, -vertOffs * i, 0);

            b.LevelData = _lvls[i];
            b.OnButtonClick += OnLevelSelected;

            _buttons[i] = b;
        }
    }

    private void OnLevelSelected(LevelButtonComp data)
    {
        GameManager.Instance.RequestLevelLoad(data.LevelData.LevelID);
    }
    public void OnBack()
    {
        GameManager.Instance.OnReturnToMain();
    }


}

