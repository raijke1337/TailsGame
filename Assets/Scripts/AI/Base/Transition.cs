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
[Serializable]
public sealed class Transition
{
    // what we evaluate
    public Decision decision;
    public State trueState;
    public State falseState;
}

