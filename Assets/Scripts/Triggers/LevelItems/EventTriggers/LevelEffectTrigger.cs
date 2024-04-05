using UnityEngine;

namespace Arcatech.Triggers
{
    public class LevelEffectTrigger : BaseLevelEventTrigger
    {
        public BaseStatTriggerConfig[] Triggers;

        protected override void OnEnter()
        {
            Debug.Log($"{this} enter {Triggers[0]}");
        }

        protected override void OnExit()
        {
            Debug.Log($"{this} exit");
        }
    }
}