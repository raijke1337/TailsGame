using Arcatech.Items;
using Arcatech.Units;
using KBCore.Refs;
using UnityEngine;

public abstract class ManagedControllerBase : ValidatedMonoBehaviour, IManagedController
{
    [SerializeField] public bool DebugMessage;
    protected bool _isReady;

    public abstract void StartController();
    public abstract void ControllerUpdate(float delta);
    public abstract void FixedControllerUpdate(float fixedDelta);
    public abstract void StopController();


    #region needsOwner

    private DummyUnit _owner;
    public DummyUnit Owner => _owner;

    public INeedsOwner SetOwner(DummyUnit owner)
    {
        _owner = owner;
        return this;
    }
    #endregion
}



public interface IManagedController : INeedsOwner
{
    public abstract void StartController();
    public abstract void ControllerUpdate(float delta);
    public abstract void FixedControllerUpdate(float fixedDelta);
    public abstract void StopController();
}