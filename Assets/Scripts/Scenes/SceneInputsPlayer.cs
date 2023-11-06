using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Arcatech.Units.Inputs
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
            _inputs.InputDirectionOverride = Vector3.forward;        // just walk forward for now
            _unit.InitiateUnit();
            _coroutine = StartCoroutine(UpdatingCoroutine());
            foreach (var id in DataManager.Instance.GetSaveData.PlayerItems.EquipmentIDs)
            {
                _unit.DrawItem(id);
            }
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