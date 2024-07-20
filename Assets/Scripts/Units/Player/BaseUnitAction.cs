using UnityEngine;


namespace Arcatech.Units
{
    public abstract class BaseUnitAction : ScriptableObject, IUnitAction
    {
        public BaseUnitAction Next;

        public abstract void DoAction(ControlledUnit user);
    }
}