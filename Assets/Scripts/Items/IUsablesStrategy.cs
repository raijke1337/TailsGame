using Arcatech.Units;

namespace Arcatech
{
    public interface IUsablesStrategy : IStrategy
    {
        BaseEntity Owner { get; }
        bool TryUseUsable(out BaseUnitAction action);
        void UpdateUsable(float delta);
    }
}