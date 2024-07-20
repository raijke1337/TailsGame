using Arcatech.Items;
using Arcatech.UI;
using Arcatech.Units;

namespace Arcatech
{
    public interface IUsable : ICosted, IActionTypeItem, IIconContent
    {
        public bool TryUseItem(out BaseUnitAction action);
        void DoUpdate(float delta);
    }

   
}