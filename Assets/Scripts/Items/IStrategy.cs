using Arcatech.Units;

namespace Arcatech
{
    public interface IUsablesStrategy : IStrategy
    {
        BaseUnit Owner { get; }
        bool TryUseItem(out BaseUnitAction action);
        void Update(float delta);
    }

    public interface IStrategy
    {
    }
}