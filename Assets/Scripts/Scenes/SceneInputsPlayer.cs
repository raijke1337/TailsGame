using Arcatech.Units;
using Arcatech.Units.Inputs;
using System.Collections;
using UnityEngine;
namespace Arcatech.Scenes
{
    public class SceneInputsPlayer : MonoBehaviour
    {
        InputsPlayer _inputs;
        PlayerUnit _unit;
        private Coroutine _coroutine;

        public void StartSceneAnimation()
        {
            _inputs = GetComponent<InputsPlayer>();
            _unit = GetComponent<PlayerUnit>();
            _unit.StartControllerUnit();
            _coroutine = StartCoroutine(UpdatingCoroutine());

            // not necessary with inventory component since the unit is init

            //foreach (var id in DataManager.Instance.GetSaveData.PlayerItems.Equipment)
            //{
            //    _unit.DrawItem(id);
            //}
        }

        protected IEnumerator UpdatingCoroutine()
        {
            while (true)
            {
                _unit.RunUpdate(Time.deltaTime);
                yield return null;
            }

        }
        private void OnDisable()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
    }
}