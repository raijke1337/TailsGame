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


public class IsoCamAdjust
{
    public Vector3 Isoforward;
    public Vector3 Isoright;

    private void AdjustDirections()
    {
        Isoforward = Camera.main.transform.forward;
        Isoforward.y = 0;
        Isoforward = Vector3.Normalize(Isoforward);
        Isoright = Quaternion.Euler(new Vector3(0, 90, 0)) * Isoforward;
    }

    public IsoCamAdjust()
    {
        AdjustDirections();
    }
}



