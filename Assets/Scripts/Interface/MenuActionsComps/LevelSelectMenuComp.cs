using Arcatech.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.UI
{
    public class LevelSelectMenuComp : MonoBehaviour
    {
        [SerializeField] RectTransform _content;
        [SerializeField] private LevelButtonComp _buttonPrefab;
        private LevelButtonComp[] _buttons;

        private void Start()
        {
            CreateButtons();
            gameObject.SetActive(false);
        }
        public void CreateButtons()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(_content);
            Assert.IsNotNull(_buttonPrefab);
#endif
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
            //float vertOffs = _buttonPrefab.GetSize.y;

            _buttons = new LevelButtonComp[_lvls.Count];


            for (int i = 0; i < _lvls.Count; i++)
            {
                var b = Instantiate(_buttonPrefab);
                //var rect = b.GetComponent<RectTransform>();
                b.transform.SetParent(_content, false);


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

}