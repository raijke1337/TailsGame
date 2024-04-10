using Arcatech.Items;
using Arcatech.Triggers.Items;
namespace Arcatech.Triggers
{
    public abstract class ControlItemTrigger : BaseLevelEventTrigger
    {
        public ControlledItem[] ControlledObject;


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
}