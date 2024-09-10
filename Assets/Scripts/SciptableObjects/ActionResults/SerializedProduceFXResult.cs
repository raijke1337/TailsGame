using Arcatech.Units;
using CartoonFX;
using UnityEngine;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New instantiate particles result ", menuName = "Actions/Action Result/Produce particle effects")]
    public class SerializedProduceFXResult : SerializedActionResult
    {
        [SerializeField] CFXR_Effect[] Effects;
        public override IActionResult GetActionResult()
        {
            return new ProduceFXResult(Effects);
        }
    }

    public class ProduceFXResult : ActionResult
    {
        CFXR_Effect[] _effs;
        public ProduceFXResult(CFXR_Effect[] effs)
        {
            _effs = effs;
        }

        public override void ProduceResult(BaseUnit user, BaseUnit target, Transform place)
        {
            foreach (var effect in _effs)
            {
                GameObject.Instantiate(effect, place.position, place.rotation);
            }
        }
    }



}