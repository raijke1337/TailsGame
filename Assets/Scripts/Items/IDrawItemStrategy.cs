using System.Collections.Generic;

namespace Arcatech.Units
{
    public interface IDrawItemStrategy : IStrategy 
    {
        public Dictionary<EquipmentType, ItemPlaceType> GetPlaces { get; }
    }


}