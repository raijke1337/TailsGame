using UnityEngine;


namespace Arcatech.Units
{
    [CreateAssetMenu(fileName = "new anim action", menuName = "Usables/Action")]
    public class UnitPlaysAnimationAction : BaseUnitAction
    {
        public string AnimationName;
        public override void DoAction(ControlledUnit user)
        {
            var a = user.GetComponent<Animator>();
            a.CrossFade(Animator.StringToHash(AnimationName), 0.1f);
        }
    }
}