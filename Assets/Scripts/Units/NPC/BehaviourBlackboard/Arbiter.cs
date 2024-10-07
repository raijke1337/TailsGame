using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Arcatech.BlackboardSystem
{
    /// <summary>
    /// arbiter class executes actions provided by IExperts, based on their Importance
    /// </summary>
    public class Arbiter
    {
        readonly List<IExpert> experts = new List<IExpert>();
        public void RegisterExpert (IExpert e)
        {
            Assert.IsNotNull(e);
            experts.Add(e);
        }
        // selecting the expert most relevant at the moment
        public List<Action> BlackboardIteration(Blackboard bb)
        {
            IExpert chosen = null;
            int highestImp = 0;
            foreach (var expert in experts)
            {
                int imp = expert.GetActionImportance(bb);
                {
                    if (imp > highestImp)
                    {
                        highestImp = imp;
                        chosen = expert;
                    }
                }
            }
            chosen?.Execute(bb);

            var actions = bb.PassedActions;
            bb.ClearActions();

            return actions;
        }
    }

}