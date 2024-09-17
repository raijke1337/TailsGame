using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    public interface IActionResult
    {
        void ProduceResult(BaseEntity user, BaseEntity target, Transform place);
    }



}