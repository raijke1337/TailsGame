using System.Threading.Tasks;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public interface IUnitAction
    {
        public void DoAction(BaseUnit user);
        public event UnityAction OnComplete;
        public bool LockMovement { get; }
    }

}