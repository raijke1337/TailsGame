public interface IManagedController
{
    
    public abstract void StartController();
    public abstract void ControllerUpdate(float delta);
    public abstract void FixedControllerUpdate(float fixedDelta);
    public abstract void StopController();
}