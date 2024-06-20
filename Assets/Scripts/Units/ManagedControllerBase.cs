using KBCore.Refs;
using UnityEngine;

public abstract class ManagedControllerBase : ValidatedMonoBehaviour
{
    [SerializeField] public bool DebugMessage;
    public virtual void StartController()
    {
        if (DebugMessage) Debug.Log($"Init controller {name}");
    }
    public abstract void UpdateController(float delta);
    public abstract void StopController();
}
