using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public class BaseUnitAction : IUnitAction
    {
        public static BaseUnitAction BuildAction(BaseUnit u, bool lck, NextActionSettings next, string anim)
        {
            return new BaseUnitAction(u,lck,next,anim);
        }
        BaseUnitAction(BaseUnit u, bool locks, NextActionSettings next, string anim)
        {
            Actor = u;
            LockMovement = locks;
            Next = next;
            _animationName = anim;
        }
        protected BaseUnit Actor; 
        public bool LockMovement { get; protected set; }
        protected NextActionSettings Next { get; }
        string _animationName;

        public bool CanAdvance(out BaseUnitAction next)
        {
            next = null;
            bool ok = Next != null && Next.CheckTime(_actionTimer.Progress); 
            if (ok)
            {
                next = Next.GetNextAction(Actor);
            }
            return ok;
        }
        CountDownTimer _actionTimer;

        public bool IsComplete { get => _actionTimer.IsReady; }

        public event UnityAction OnComplete = delegate { };

        public void DoAction(BaseUnit user)
        {
            Debug.Log($"start action anim {_animationName}");

            var a = user.GetComponent<Animator>();
            a.CrossFade(Animator.StringToHash(_animationName), 0.1f);

            var time = a.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            _actionTimer = new CountDownTimer(time);
            _actionTimer.Start();

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