using UnityEngine;
using static Arcatech.Units.Behaviour.Node;

namespace Arcatech.Units.Behaviour
{
    public class IdleAtPointStrategy : IBehaviorTreeStrategy
    {
        readonly float waitTime;
        CountDownTimer waitTimer;

        public IdleAtPointStrategy (float waitTime)
        {
            this.waitTime = waitTime;
            waitTimer = new CountDownTimer(waitTime);
        }

        public NodeStatus Process(ControlledUnit actor)
        {
            if (waitTimer.IsReady)
            {
                return NodeStatus.Success;
            }

            if (!waitTimer.IsRunning)
            {
                waitTimer.Start();
            }
            if (waitTimer.IsRunning) 
            {
                waitTimer.Tick(Time.deltaTime);
                
                return NodeStatus.Running;
            }

            else return NodeStatus.Fail;
        }

        public void Reset()
        {
            waitTimer.Reset();
        }
    }
}