using Arcatech.Triggers;

namespace Arcatech.Items
{
    public interface ICostedItem
    {
        public StatsEffect GetCost { get; }
    }

   
}