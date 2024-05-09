using Arcatech.Items;
using Arcatech.Triggers.Items;
using Arcatech.Units;

namespace Arcatech.Triggers
{
    public abstract class ControlItemTrigger : BaseLevelEventTrigger
    {
        public ControlledItem[] ControlledObject;
        public ControlItemCondition Condition;

        public virtual void DoPositiveAction()
        {
            foreach (var i in ControlledObject)
            {
                i.ChangeItemState(ControlledItemState.Positive);
            }
            
        }
        public virtual void DoNegativeAction()
        {
            foreach (var i in ControlledObject)
            {
                i.ChangeItemState(ControlledItemState.Negative);
            }
        }

    }

    // todo turn it into a scriptable object maybe?
    // abstract bool DoTest()
    public class ControlItemCondition
    {
        public BaseUnit UnitToKill;
        public Item CheckItemInInventory;

    }

}