using UnityEngine;

namespace Arcatech.BlackboardSystem
{
    public class UnitBlackBoardController : MonoBehaviour
    {/// <summary>
    /// CODE IS USED IN UNIT MANAGER
    /// </summary>

        [SerializeField] BlackboardData bbData;
        readonly Blackboard bb = new();
        readonly ActionPicker ar = new ActionPicker();

        private void Awake()
        {
            bbData.SetupBlackboard(bb);

        }
        public Blackboard GetBlackboard => bb;
        public void RegisterExpert(IRoomUnitTacticsMember e) => ar.RegisterExpert(e);

        private void Update()
        {
            foreach (var act in ar.BlackboardIteration(bb))
            {
                act();
            }
        }

    }

}