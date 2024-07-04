using Arcatech.UI;

namespace Arcatech.Items
{
    public interface IUsableItem : ICostedItem, IActionTypeItem, IIconContent
    {
        public bool TryUseItem();
    }

   
}