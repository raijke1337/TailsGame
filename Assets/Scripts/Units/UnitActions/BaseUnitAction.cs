using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public class BaseUnitAction : IUnitAction
    {
        public static BaseUnitAction BuildAction(BaseUnit u, bool lck, NextActionSettings next, string anim, float exit)
        {
            return new BaseUnitAction(u, lck, next, anim, exit);
        }
        BaseUnitAction(BaseUnit u, bool locks, NextActionSettings next, string anim, float exitTime)
        {
            Actor = u;
            LockMovement = locks;
            Next = next;
            _animationName = anim;
            _exitTime = exitTime;

            var a = u.GetComponent<Animator>();
            var clip = a.runtimeAnimatorController.animationClips.First(t => t.name == _animationName);
            animTime = clip.length;

            AnimatorController ac = a.runtimeAnimatorController as AnimatorController;
            var l = ac.layers;

            foreach (var layer in l)
            {
                var s = layer.stateMachine.states;
                foreach (var state in s)
                {
                    if (state.state.name == _animationName)
                    {
                        animSpeedMult = state.state.speed;
                        //Debug.Log($"found anim speed {animSpeedMult} for animation {_animationName}");
                        break;
                    }
                }
            }

        }
        protected BaseUnit Actor;
        public bool LockMovement { get; protected set; }
        protected NextActionSettings Next { get; }
        public event UnityAction OnComplete = delegate { };


        string _animationName;
        protected float _exitTime = 1f;
        CountDownTimer _actionTimer;
        float animSpeedMult = 1f;
        float animTime;


        public bool CanAdvance(out BaseUnitAction next)
        {
            next = null;
            Debug.Log($"Check anim {_animationName}, progress {_actionTimer.Progress}, exit time is {_exitTime}");
            bool ok = Next != null && Next.CheckTime(_actionTimer.Progress);
            if (ok)
            {
                next = Next.GetNextAction(Actor);
            }
            return ok;
        }


        public bool IsComplete
        {
            get => _actionTimer == null || _actionTimer.Progress >= _exitTime || _actionTimer.IsReady; 
        }


        public void DoAction(BaseUnit user)
        {

            var a = user.GetComponent<Animator>();
            a.CrossFade(Animator.StringToHash(_animationName), 0.1f);

            _actionTimer = new CountDownTimer(animTime/animSpeedMult);
            _actionTimer.Start();

            //Debug.Log($"clip {_animationName}, timer set to {animTime/animSpeedMult}");
            _actionTimer.OnTimerStopped += CompleteAction;
        }

        protected void CallComplete() => OnComplete.Invoke();
        void CompleteAction()
        {
            CallComplete();
        }

        public void Update(float delta)
        {
            _actionTimer?.Tick(delta);
        }

        public override string ToString()
        {
            return _animationName;
        }
    }

    [Serializable]
    public class NextActionSettings
    {
        [SerializeField] SerializedUnitAction _nextAnim;
        [SerializeField,Range(0f, 1f)] float _chainWindowStart;
        [SerializeField,Range(0f, 1f)] float _chainWindowEnd;
        public bool CheckTime(float currentPercent)
        {
            return (currentPercent >= _chainWindowStart && currentPercent <= _chainWindowEnd);
        }
        public BaseUnitAction GetNextAction(BaseUnit unit) => _nextAnim.ProduceAction(unit);
    }
}