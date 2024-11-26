using Arcatech.Stats;
using Arcatech.Triggers;
using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
namespace Arcatech.Level
{
    public class PassiveEventTrigger : MonoBehaviour
    {
        [SerializeField] protected EventCondition check;
        [SerializeField] protected ConditionControlledItem affected;

        public bool Triggered { get; protected set; } = false;

        IInteractible checkdObject;

        private void Start()
        {
            checkdObject = GetComponent<IInteractible>();
        }
        public void CheckEventTrigger()
        {
            if (checkdObject == null || Triggered) return;
            if (check.PerformConditionChecks(null, checkdObject, transform))
            {
                affected.SetState(true);
                Triggered = true;
            }

        }
    }
    }
