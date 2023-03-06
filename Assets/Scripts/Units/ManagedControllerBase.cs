using UnityEngine;

public abstract class ManagedControllerBase : MonoBehaviour
{
    public abstract void StartController();
    public abstract void UpdateController(float delta);
    public abstract void StopController();
}
