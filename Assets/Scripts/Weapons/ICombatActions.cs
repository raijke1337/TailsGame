namespace Arcatech
{
    /// <summary>
    /// for controller components
    /// </summary>
    public interface ICombatActions : IUsesStats
    {
        public bool TryUseAction (UnitActionType action);
    }
}