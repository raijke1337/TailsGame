﻿using Arcatech.Actions;
using System;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public class BaseUnitAction : IUnitAction
    {
        public static BaseUnitAction BuildAction(BaseEntity u, bool lck, NextActionSettings next, string anim, float exit, SerializedActionResult[] onstart, SerializedActionResult[] onfinish, SerializedActionResult[] onExit, Transform place)
        {
            return new BaseUnitAction(u, lck, next, anim, exit,onstart, onfinish, onExit,place);
        }

        BaseUnitAction(BaseEntity u, bool locks, NextActionSettings next, string anim, float exitTimeMult, SerializedActionResult[] onstart, SerializedActionResult[] onfinish, SerializedActionResult[] onExit, Transform place)
        {
            Actor = u;
            LockMovement = locks;
            Next = next;
            _animationName = anim;

            this.place = place;
            var a = u.GetComponent<Animator>();

            if (_animationName != null && a.runtimeAnimatorController.animationClips.Any(t=>t.name == _animationName))
            {
                var clip = a.runtimeAnimatorController.animationClips.First(t => t.name == _animationName);
                var clipLength = clip.length;

                AnimatorController ac = a.runtimeAnimatorController as AnimatorController;
                var l = ac.layers;

                foreach (var layer in l)
                {
                    var s = layer.stateMachine.states;
                    foreach (var state in s)
                    {
                        if (state.state.name == _animationName)
                        {
                            var animSpeedMult = state.state.speed;
                            //Debug.Log($"found anim speed {animSpeedMult} for animation {_animationName}");

                            _totalActionTime = clipLength / animSpeedMult;
                            _exitActionTime = _totalActionTime * exitTimeMult;
                           if (u.UnitDebug) Debug.Log($"Action {this} exit at {_exitActionTime} complete at {_totalActionTime}");
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"animation not found for action {this}");
            }




            if (onstart != null && onstart.Length > 0)
            {
                OnStartAction = new IActionResult[onstart.Length]; 
                for (int i = 0; i < onstart.Length; i++)
                {
                    OnStartAction[i] = onstart[i].GetActionResult();
                }
            }
            if (onfinish != null && onfinish.Length > 0)
            {
                OnCompleteAction = new IActionResult[onfinish.Length];
                for (int i = 0; i < onfinish.Length; i++)
                {
                    OnCompleteAction[i] = onfinish[i].GetActionResult();
                }
            }
            if (onExit != null && onExit.Length > 0)
            {

                OnExitTime = new IActionResult[onExit.Length];
                for (int i = 0; i < onExit.Length; i++)
                {
                    OnExitTime[i] = onExit[i].GetActionResult();
                }
            }

            _actionTimer = new StopwatchTimer();
        }
       
        
        
        protected BaseEntity Actor;
        public bool LockMovement { get; protected set; }
        protected NextActionSettings Next { get; }


        StopwatchTimer _actionTimer;
        readonly float _totalActionTime;
        readonly float _exitActionTime;

        readonly Transform place;
        readonly string _animationName;



        UnitActionState _actionState = UnitActionState.None;
        public UnitActionState GetActionState { get { return _actionState; } }

        readonly IActionResult[] OnCompleteAction;
        readonly IActionResult[] OnStartAction;
        readonly IActionResult[] OnExitTime;

        public UnitActionState UpdateAction(float delta)
        {
            _actionTimer?.Tick(delta);
            
            if (_actionTimer.GetTime >= _exitActionTime && _actionState == UnitActionState.Started)
            {
                ExitTimeAction();
            }
            if (_actionTimer.GetTime >= _totalActionTime && _actionState == UnitActionState.ExitTime)
            {
                CompleteAction();
            }
            return _actionState;
        }

        public bool CanAdvance(out SerializedUnitAction next)
        {
            next = null;

            bool ok = Next != null && Next.CheckTime(_actionTimer.GetTime / _totalActionTime);
            if (ok)
            {
                next = Next.GetNextAction;
            }
            return ok;
        }

        public void StartAction()
        {
            string start = "";
            var a = Actor.GetComponent<Animator>();
            if (_animationName!= null)
            {
                a.CrossFade(Animator.StringToHash(_animationName), 0.1f);
            }
            if (OnStartAction != null)
            {
                foreach (var r in OnStartAction)
                {
                    if (r == null)
                    {
                        start += "NULL RESULT";
                    }// bandaid TODO dunno why it happens
                    else
                    {
                        r.ProduceResult(Actor, null, place);
                        start += (r.ToString() + ' ');
                    }
                }
            }
            _actionTimer.Reset();
            _actionTimer.Start();
            _actionState = UnitActionState.Started;

           // if (Actor.UnitDebug) { Debug.Log($"{this}, result {start}"); }

        }
        void ExitTimeAction()
        {
            string ex = "";
            if (OnExitTime != null)
            {
                foreach (var r in OnExitTime)
                {
                    if (r == null)
                    {
                        ex += "NULL RESULT";
                    }// bandaid TODO dunno why it happens
                    else
                    {
                        r.ProduceResult(Actor, null, place);
                        ex += (r.ToString() + ' ');
                    }
                }
            }
            _actionState = UnitActionState.ExitTime;
            if (Actor.UnitDebug) { Debug.Log($"{this}, result {ex}"); }

        }
        public void CompleteAction()
        {
            if (_actionState == UnitActionState.Completed)
            {
                _actionState = UnitActionState.None;
                return;
            }
            string fin = "";
            if (OnCompleteAction != null)
            {
                foreach (var r in OnCompleteAction)
                {
                    if (r == null)
                    {
                        fin +=  "NULL RESULT";
                    }// bandaid TODO dunno why it happens
                    else
                    {
                        r.ProduceResult(Actor, null, place);
                        fin += (r.ToString() + ' ');
                    }
                }
            };
            _actionState = UnitActionState.Completed;
            _actionTimer.Stop();

            if (Actor.UnitDebug) { Debug.Log($"{this}, result {fin}"); }

            //OnComplete.Invoke();
        }
        public override string ToString()
        {
            return _animationName+ " state: "+_actionState;
        }

    }

    [Serializable]
    public class NextActionSettings
    {
        [SerializeField] SerializedUnitAction _nextAnim;
        [SerializeField,Range(0f, 1f)] float _chainWindowEnd;
        public bool CheckTime(float currentPercent)
        {
            return (currentPercent <= _chainWindowEnd);
        }
        public SerializedUnitAction GetNextAction { get => _nextAnim; }
    }
}