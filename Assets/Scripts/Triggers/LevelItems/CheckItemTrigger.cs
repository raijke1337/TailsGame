using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers.Items;
namespace Arcatech.Triggers
{

    public class CheckItemTrigger : CheckConditionTrigger
    {
        public Item RequiredItem;

        protected override bool CheckTheCondition()
        {
            return GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit.GetInventoryComponent.HasItem(RequiredItem);
        }
    }
}