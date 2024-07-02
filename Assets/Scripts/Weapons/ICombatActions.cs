namespace Arcatech
{
    /// <summary>
    /// for controller components
    /// </summary>
    public interface ICombatActions : IManagedController
    {
        public bool TryUseAction (UnitActionType action);
    }
}