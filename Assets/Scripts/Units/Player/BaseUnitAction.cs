using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public abstract class BaseUnitAction : IUnitAction
    {
        protected BaseUnit Actor;
        protected SerializedUnitAction Next;
        public event UnityAction OnComplete = delegate { };

        public BaseUnitAction NextAction
        {
            get
            {
                if (Next == null) return null;
                else
                {
                    return Next.ProduceAction(Actor);
                }
            }
        }
        public BaseUnitAction (BaseUnit u, SerializedUnitAction next, bool locks)
        {
            Next = next;
            Actor = u;
            LocksInputs = locks;
            IsDone = true;
        }

        public abstract void DoAction(BaseUnit user);
        public bool LocksInputs { get; protected set; }
        protected void CallComplete() => OnComplete.Invoke();
        public bool IsDone { get; protected set; }
        public abstract void Update(float delta);

    }



    public abstract class SerializedUnitAction : ScriptableObject
    {
        [SerializeField] protected bool _locksInputs;
        [SerializeField] protected SerializedUnitAction Next;
        public abstract BaseUnitAction ProduceAction(BaseUnit unit);
    }
}