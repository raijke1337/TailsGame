using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;


namespace Arcatech.Units
{
    public class ActionsHandler
    {
        [SerializeField] SerializedDictionary<UnitActionType, BaseUnitAction> _actions;
        ControlledUnit Owner { get; }

        public ActionsHandler(ControlledUnit owner, BaseUnitAction jump)
        {
            _actions = new();
            _actions[UnitActionType.Jump] = jump;
            Owner = owner;
        }

        public void OnCommand (UnitActionType act)
        {
            if (_actions.TryGetValue(act, out BaseUnitAction action))
            {
                action.DoAction(Owner);

            }            
        }    
    }


}