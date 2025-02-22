﻿using Arcatech.Triggers;

namespace Arcatech.Items
{
    public interface IEquippable : IHasSkill, IItem
    {
        public BaseEquippableItemComponent DisplayItem { get; }
        public SerializedStatModConfig[] StatMods { get; }
    
    }

    public interface IItem
    {
        public EquipmentType Type { get; }
    }
}