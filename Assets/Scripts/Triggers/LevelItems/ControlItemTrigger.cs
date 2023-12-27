using Arcatech.Items;
using Arcatech.Triggers.Items;
namespace Arcatech.Triggers
{
    public abstract class ControlItemTrigger : BaseLevelEventTrigger
    {
        public ControlledItem ControlledObject;


        public virtual void DoPositiveAction()
        {
            ControlledObject.DoControlAction(true);
        }
        public virtual void DoNegativeAction()
        {
            ControlledObject.DoControlAction(false);
        }

    }
}