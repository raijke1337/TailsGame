using Arcatech.Units;
using Arcatech.Units.Inputs;
using UnityEngine;

public class IntroSceneScript : MonoBehaviour
{
    [SerializeField] PlayerUnit _player;
    private SceneInputsPlayer _playerInputs;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _camera.transform.SetParent(_player.transform, true);
        _playerInputs = _player.gameObject.AddComponent<SceneInputsPlayer>();
        _playerInputs.StartSceneAnimation();
    }
}
