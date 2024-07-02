using Arcatech.Items;
using Arcatech.Managers;
namespace Arcatech.Triggers
{

    public class CheckItemTrigger : CheckConditionTrigger
    {
        public ItemSO RequiredItem;

        protected override bool CheckTheCondition()
        {
            return DataManager.Instance.PlayerHasItem(RequiredItem);
        }
   
    }

}