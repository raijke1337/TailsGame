using System.Threading.Tasks;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public interface IUnitAction
    {
        public void StartAction();
        public UnitActionState UpdateAction(float delta);
        public bool LockMovement { get; }
    }


    public enum UnitActionState
    {
        None,
        Started,
        ExitTime,
        Completed
    }
}