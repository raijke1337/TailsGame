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

public class ReflectionTestin : MonoBehaviour
{

    private void Start()
    {
        Type _t = typeof(ReflectionTestin);
        foreach (var m in _t.GetMethods())
        {
            Debug.Log($"Found {m} in {_t},running:");
            m.Invoke(this, null);           
        }
    }
    private void ExampleMethod() // private does not run!
    {
        Debug.Log("ASS");
    }
    

}

public class Testing
{
    public delegate void TestDelegate(string info);
    TestDelegate? text;
    public event TestDelegate PrintTextEvent
    {
        add { text += value; } 
        remove { text -= value; }

    }


}

