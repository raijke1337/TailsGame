using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    public interface IActionCondition
    {
        bool PerformConditionChecks(BaseEntity user, BaseEntity target, Transform place); // just in case
    }

}