using Arcatech.Units;

namespace Arcatech
{
    public interface IUsablesStrategy : IStrategy
    {
        BaseUnit Owner { get; }
        bool TryUseUsable(out BaseUnitAction action);
        void UpdateUsable(float delta);
    }

    public interface IStrategy
    {
    }
}