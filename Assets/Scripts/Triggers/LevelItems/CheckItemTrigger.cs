using Arcatech.Items;
using Arcatech.Managers;
namespace Arcatech.Triggers
{

    public class CheckItemTrigger : CheckConditionTrigger
    {
        public Item RequiredItem;

        protected override bool CheckTheCondition()
        {
            return GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit.GetInventoryComponent.HasItem(RequiredItem.ID, out _);
        }
   
    }

}