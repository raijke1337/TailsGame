using System.Collections.Generic;
using System.Linq;
namespace Arcatech.Units.Behaviour
{
    public class BehaviourRandomSelector : BehaviourPrioritySelector
    {
        public BehaviourRandomSelector(string name, int p = 0) : base(name, p)
        {
        }

        protected override List<Node> SortChildren()
        {
            return Children.Shuffle().ToList();
        }
    }


}