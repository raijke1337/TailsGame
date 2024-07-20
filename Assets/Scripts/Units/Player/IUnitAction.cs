namespace Arcatech.Units
{
    public interface IUnitAction
    {
        public void DoAction(ControlledUnit user);
    }
    public abstract class UnitAction : IUnitAction
    {
        public abstract void DoAction(ControlledUnit user);
    }

}