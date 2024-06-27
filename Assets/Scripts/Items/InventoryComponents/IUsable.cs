namespace Arcatech.Items
{
    public interface IUsableItem : ICostedItem, IActionTypeItem
    {
        public void UseItem();
        public IUsableItem AssignStrategy();
    }

   
}