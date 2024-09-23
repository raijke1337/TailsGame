using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.UI;
using Arcatech.Units;

namespace Arcatech
{
    public interface IUsable : ICosted, IActionTypeItem, IIconContent
    {
        public bool TryUseItem(UnitStatsController stats, out BaseUnitAction action);
        void DoUpdate(float delta);
    }

   
}