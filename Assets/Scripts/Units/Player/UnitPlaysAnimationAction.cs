using System;
using System.Threading.Tasks;
using UnityEngine;


namespace Arcatech.Units
{
    [CreateAssetMenu(fileName = "new anim action", menuName = "Usables/Action")]
    public class UnitPlaysAnimationAction : SerializedUnitAction
    {
        public string AnimationName;
        public override BaseUnitAction ProduceAction(BaseUnit unit)
        {
            return new UnitAnimatesAction(AnimationName, unit, Next, _locksInputs);
        }

    }

    public class UnitAnimatesAction : BaseUnitAction
    {
        string AnimationName;

        public UnitAnimatesAction(string name, BaseUnit u, SerializedUnitAction next, bool locks) : base(u, next, locks)
        {
            AnimationName = name;
        }

        CountDownTimer _lockTimer;
        public override void DoAction(BaseUnit user)
        {
            Debug.Log($"start action anim {AnimationName}");
            IsDone = false;
            var a = user.GetComponent<Animator>();
            a.CrossFade(Animator.StringToHash(AnimationName), 0.1f);
            var time = a.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            _lockTimer = new CountDownTimer(time);
            _lockTimer.Start();
            _lockTimer.OnTimerStopped += CompleteAction;
        }
        public override void Update(float delta)
        {
           // Debug.Log($"Action {AnimationName} done : {_lockTimer.Progress}");
            _lockTimer?.Tick(delta);
        }
        void CompleteAction()
        {
            CallComplete();
            IsDone = true;
            Debug.Log($"complete action anim {AnimationName}");
        }
    }
}