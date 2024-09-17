using Arcatech.Units;
using CartoonFX;
using UnityEngine;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New instantiate particles result ", menuName = "Actions/Action Result/Produce particle effects")]
    public class SerializedProduceFXResult : SerializedActionResult
    {
        [SerializeField] CFXR_Effect[] Effects;
        [SerializeField] bool ParentParticles;
        public override IActionResult GetActionResult()
        {
            return new ProduceFXResult(Effects, ParentParticles);
        }
    }

    public class ProduceFXResult : ActionResult
    {
        CFXR_Effect[] _effs;
        bool parent;
        public ProduceFXResult(CFXR_Effect[] effs, bool p)
        {
            _effs = effs;
            parent = p;
        }

        public override void ProduceResult(BaseEntity user, BaseEntity target, Transform place)
        {
            Transform par = null;
            if (parent) par = target.transform;
            foreach (var effect in _effs)
            {
                GameObject.Instantiate(effect, place.position, place.rotation,par);
            }
        }
    }



}