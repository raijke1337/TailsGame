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

public class UnitController : MonoBehaviour
{
    public ref Vector3 MoveDirection => ref _movement;
    protected Vector3 _movement;

    

    public SimpleEventsHandler OnMeleeAttack;
    public SimpleEventsHandler OnQSpecial;
    public void CallOnMeleeAttack() => OnMeleeAttack?.Invoke();
    public void CallOnQSpecial() => OnQSpecial?.Invoke();


    public Vector3 Isoforward;
    public Vector3 Isoright;
    protected virtual void Awake()
    {
        AdjustDirections();
    }
    private void AdjustDirections()
    {
        Isoforward = Camera.main.transform.forward;
        Isoforward.y = 0;
        Isoforward = Vector3.Normalize(Isoforward);
        Isoright = Quaternion.Euler(new Vector3(0, 90, 0)) * Isoforward;
    }


}

