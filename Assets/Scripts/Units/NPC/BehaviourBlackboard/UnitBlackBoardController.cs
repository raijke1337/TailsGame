using UnityEngine;

namespace Arcatech.BlackboardSystem
{
    public class UnitBlackBoardController : MonoBehaviour
    {/// <summary>
    /// CODE IS USED IN UNIT MANAGER
    /// </summary>

        [SerializeField] BlackboardData bbData;
        readonly Blackboard bb = new();
        readonly Arbiter ar = new Arbiter();

        private void Awake()
        {
            bbData.SetupBlackboard(bb);

        }
        public Blackboard GetBlackboard => bb;
        public void RegisterExpert(IExpert e) => ar.RegisterExpert(e);

        private void Update()
        {
            foreach (var act in ar.BlackboardIteration(bb))
            {
                act();
            }
        }

    }

}