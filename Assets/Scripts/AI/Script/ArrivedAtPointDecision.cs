using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Decision/ArrivedAtPoint")]
public class ArrivedAtPointDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        var result = Vector3.Distance(controller.NMAgent.transform.position,
            controller.PatrolPoints[controller.CurrentPatrolPointIndex].position)
            < controller.NMAgent.stoppingDistance;
        if (result)
        {
            controller.OnPatrolPointReached();
        }
        return result;
    }
}

