using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "AIConfig/Action/Attacking/Generic Attack")]
public class GenericAttackAction : Action
{
    public override void Act(StateMachine controller)
    {
        AttackLogic(controller);
    }
    protected virtual void AttackLogic(StateMachine controller)
    { controller.OnAttackRequest(CombatActionType.Melee);  }
    

}

