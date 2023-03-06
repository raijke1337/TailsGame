using UnityEngine;
using Zenject;

public class EventSystemBandaidController : MonoBehaviour
{
    [Inject]
    private PlayerUnit _player;

    private void LateUpdate()
    {
        gameObject.transform.position = _player.transform.position;
    }
}

