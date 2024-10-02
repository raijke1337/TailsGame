using Arcatech.Actions;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Level
{
    [CreateAssetMenu(fileName = "New dummy check (enter trigger)", menuName = "Game/Level events/Condition/Dummy - Always succeed", order = 1)]
    public class SerializedActionCondition : ScriptableObject, IActionCondition
    {
        [SerializeField] SerializedActionResult[] OnSuccess;
        [SerializeField] SerializedActionResult[] OnFail; 

        public virtual bool PerformConditionChecks(BaseEntity user, BaseEntity target, Transform place)
        {
            bool testResult = true;


            ProduceResults(testResult,  user,  target,  place);

            return testResult;
        }

        void ProduceResults(bool ok, BaseEntity user, BaseEntity target, Transform place)
        {

            if (ok)
            {
                if (OnSuccess == null) return;
                foreach (var item in OnSuccess) { if (item == null) return; else item.GetActionResult().ProduceResult(user, target, place); }
            }
            else
            {
                if (OnFail == null) return;
                foreach (var item in OnFail) { if (item == null) return; else  item.GetActionResult().ProduceResult(user, target, place); }
            }
        }
    }

}