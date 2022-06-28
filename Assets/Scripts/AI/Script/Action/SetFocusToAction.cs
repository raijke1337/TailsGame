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
[CreateAssetMenu(menuName = "AIConfig/Action/SetFocus")]
public class SetFocusToAction : Action
{
    public UnitType DesiredType;
    public override void Act(StateMachine controller)
    {
        controller.OnSetFocus(DesiredType);
    }
}

