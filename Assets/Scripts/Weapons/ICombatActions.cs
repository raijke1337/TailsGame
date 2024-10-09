using Arcatech.Units;
using System.Collections.Generic;

namespace Arcatech
{
    /// <summary>
    /// for controller components
    /// </summary>
    public interface ICombatActions : IManagedController
    {
        public bool CanUseAction(UnitActionType action);
        public bool TryUseAction(UnitActionType action, out BaseUnitAction onUse);
    }
}