using UnityEngine;

namespace Arcatech.Units.Behaviour
{
    public class Leaf : Node // leaf is a node without children but with behavior to execute
    {
        readonly IBehaviorTreeStrategy strat;
        protected BehaviorTreeDebugger _debug;

        public Leaf(IBehaviorTreeStrategy strat, string n, int p = 0) : base (n,p)
        {
            this.strat = strat;
            
        }

        public override NodeStatus Process(NPCUnit actor)
        {
            var st = strat.Process(actor);
            if (actor.UnitDebug)
            {
                if (_debug == null) _debug = new BehaviorTreeDebugger();
                _debug.PrintDebug($"{actor.GetName}: {NodeName} status {st}");
            }
            return st;
        }
        public override void Reset()
        {
            strat.Reset();
        }
    }


}