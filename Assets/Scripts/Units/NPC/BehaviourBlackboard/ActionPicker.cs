using Arcatech.AI;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Arcatech.BlackboardSystem
{
    /// <summary>
    /// action picker class executes actions provided by IRoomUnitTacticsMember, based on their Importance
    /// </summary>
    public class ActionPicker
    {
        readonly List<IRoomUnitTacticsMember> experts = new List<IRoomUnitTacticsMember>();
        public void RegisterExpert (IRoomUnitTacticsMember e)
        {
            Assert.IsNotNull(e);
            experts.Add(e);
        }
        // selecting the expert most relevant at the moment
        //public List<Action> BlackboardIteration(Blackboard bb)
        //{
        //    IRoomUnitTacticsMember chosen = null;
        //    int highestImp = 0;
        //    foreach (var expert in experts)
        //    {
        //        int imp = expert.GetActionImportance(bb);
        //        {
        //            if (imp > highestImp)
        //            {
        //                highestImp = imp;
        //                chosen = expert;
        //            }
        //        }
        //    }
        //    chosen?.Execute(bb);

        //    var actions = bb.PassedActions;
        //    bb.ClearActions();

        //    return actions;
        //}
    }

}