using UnityEngine;

public class CrosshairComponent : MonoBehaviour
{
    public void SetScreenPosition(Vector3 position) => transform.position = position;

    private SpriteRenderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void SetCrosshair(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
}

