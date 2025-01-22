using System.Collections;
using UnityEngine;

namespace Arcatech.BlackboardSystem
{
    /// <summary>
    /// expert is a class that makes a decision regarding a certain aspect of behavior
    /// </summary>
    public interface IRoomUnitTacticsMember 
    {
        int GetActionImportance(Blackboard bb);
        void Execute(Blackboard bb);
    }


}