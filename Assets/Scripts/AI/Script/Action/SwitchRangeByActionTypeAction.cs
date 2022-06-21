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
[CreateAssetMenu(menuName = "AIConfig/Action/Switch Range by desired combat action")]

public class SwitchRangeByActionTypeAction : Action
{
    public CombatActionType DesiredType;
    public override void Act(StateMachine controller)
    {
        throw new NotImplementedException();
    }
}

