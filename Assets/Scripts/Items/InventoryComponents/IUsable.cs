using Arcatech.Items;
using Arcatech.UI;

namespace Arcatech
{
    public interface IUsable : ICosted, IActionTypeItem, IIconContent
    {
        public bool TryUseItem();
        void DoUpdate(float delta);
    }

   
}