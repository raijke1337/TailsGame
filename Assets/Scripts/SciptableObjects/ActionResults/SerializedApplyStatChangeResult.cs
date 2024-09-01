using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
using Arcatech.EventBus; 

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New apply stat change result", menuName = "Actions/Action Result/Apply Stat Change", order = 3)]
    public class SerializedApplyStatChangeResult : SerializedActionResult
    {
        [SerializeField] SerializedStatsEffectConfig[] StatChanges;
        public override IActionResult GetActionResult()
        {
            return new ApplyStatChangeEffectResult(StatChanges);
        }
    }
    public class ApplyStatChangeEffectResult : ActionResult
    {
        StatsEffect[] _effs; 
        public ApplyStatChangeEffectResult(SerializedStatsEffectConfig[] effects)
        {
            _effs = new StatsEffect[effects.Length];
            for (int i = 0; i < effects.Length; i++)
            {
                _effs[i] = new StatsEffect(effects[i]);
            }
        }

        public override void ProduceResult(BaseUnit user, BaseUnit target,Transform place)
        {
            Debug.Log($"Result: applying {_effs.Length} effects");
            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target, user, true, place, _effs));
        }
    }

}