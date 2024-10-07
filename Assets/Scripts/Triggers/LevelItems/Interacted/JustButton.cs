using KBCore.Refs;
using UnityEngine;

namespace Arcatech.Level
{
    [RequireComponent(typeof(Animator))]
    public class JustButton : ConditionControlledItem
    {
        [SerializeField] Animator _a;
            private void OnValidate()
        {
            _a = GetComponent<Animator>();
        }
        protected override void OnSetState(bool newstate)
        {
            _a.SetTrigger("Use");
        }
    }


}