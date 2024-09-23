namespace Arcatech.Units.Behaviour
{
    public class BehaviorUntilFail : Node
    {
        public BehaviorUntilFail(string n, int p = 0) : base(n, p) { }

        public override NodeStatus Process(NPCUnit actor)
        {
            if (Children[currentChildIndex].Process(actor) == NodeStatus.Fail)
            {
                Reset();
                return NodeStatus.Fail;
            }
            else return NodeStatus.Running;
        }
    }

}