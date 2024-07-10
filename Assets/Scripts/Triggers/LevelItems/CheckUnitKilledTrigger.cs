using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUnitKilledTrigger : CheckConditionTrigger
{
    [SerializeField] protected BaseUnit _unit;

    protected override bool CheckTheCondition()
    {
        return true; // TODO
    }

}
