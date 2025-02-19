﻿using Arcatech.Stats;
using System.Collections.Generic;

public interface ITargetable
{
    public string UnitName { get; }
    public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetDisplayValues { get; }
}