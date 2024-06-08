using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Triggers
{
    public abstract class CheckConditionTrigger : BaseLevelEventTrigger
    {
        [SerializeField,Tooltip("Item and desired state")] protected SerializedDictionary<ConditionControlledItem, ControlledItemsStatesPackage> _controlledItems;
        public event ConditionCheckEvents UpdatedConditionStateEvent;

        public bool GetCondition { get => CheckTheCondition(); }
        public void UpdateControlledItems(bool st)
        {
            foreach (var item in _controlledItems.Keys)
            {
                item.ChangeItemState(_controlledItems[item].States[st]);
            }
        }

        protected abstract bool CheckTheCondition();

    }


    [Serializable]
    public class ControlledItemsStatesPackage
    {
        public SerializedDictionary<bool, ControlledItemState> States;
    }


}