using Arcatech.Actions;
using Arcatech.Effects;
using Arcatech.Units;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Assertions;
namespace Arcatech.Triggers
{


    public class BaseLevelEventTrigger : BaseTrigger
    {
        [Header("Base event trigger")]
        [SerializeField] protected TriggerTargetType targetType;
        [SerializeField] protected bool DestroyOnExit = false;
        [SerializeField] protected bool DestroyOnEnter = false;
        [Space, SerializeField] protected SerializedActionResult[] ActionOnEntry;
        [SerializeField] protected SerializedActionResult[] ActionOnExit;


        private void OnValidate()
        {
            Assert.IsFalse(targetType == TriggerTargetType.None);
        }

        protected override void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.TryGetComponent(out BaseEntity p))
            {
                switch (targetType)
                {
                    case TriggerTargetType.AnyUnit:
                        ApplyResults(p);
                        break;
                    case TriggerTargetType.OnlyUser:
                        ApplyResults(p);
                        break;
                    case TriggerTargetType.AnyEnemy:
                        if (p.Side == Side.EnemySide) ApplyResults(p);
                        break;
                    case TriggerTargetType.AnyAlly:
                        if (p.Side == Side.PlayerSide) ApplyResults(p);
                        break;
                    default:
                        Debug.Log($"{p.GetName} entered {this} and nothing happened because of trigger settings");
                        break;
                }
            }

            if (DestroyOnEnter)
            {
                gameObject.SetActive(false);
            }
        }


        protected override void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out BaseEntity p))
            {
                switch (targetType)
                {
                    case TriggerTargetType.AnyUnit:
                        ApplyResults(p);
                        break;
                    case TriggerTargetType.OnlyUser:
                        ApplyResults(p);
                        break;
                    case TriggerTargetType.AnyEnemy:
                        if (p.Side == Side.EnemySide) ApplyResults(p);
                        break;
                    case TriggerTargetType.AnyAlly:
                        if (p.Side == Side.PlayerSide) ApplyResults(p);
                        break;
                    default:
                        Debug.Log($"{p.GetName} exited {this} and nothing happened because of trigger settings");
                        break;
                }
            }

            if (DestroyOnExit)
            {
                gameObject.SetActive(false);
            }
        }


        protected void ApplyResults(BaseEntity p)
        {
            foreach (var action in ActionOnEntry)
            {
                action.GetActionResult().ProduceResult(null, p, transform);
            }

            if (DestroyOnEnter)
            {
                gameObject.SetActive(false);
            }
        }
    }
}