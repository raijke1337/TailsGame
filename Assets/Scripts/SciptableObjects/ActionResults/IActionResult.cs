using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    public interface IActionResult
    {
        void ProduceResult(BaseUnit user, BaseUnit target, Transform place);
    }



}