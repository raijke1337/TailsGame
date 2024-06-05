using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AI System/Decision/Assess/Distance To Point")]
    public class AssessRangeDecision : Decision
    {
        public float DesiredRange;
        public Vector3 CheckedPosition;
        public override bool Decide(StateMachine controller)
        {
            return (Vector3.Distance(controller.ControlledUnit.transform.position,CheckedPosition) <= DesiredRange);
        }
    }
}