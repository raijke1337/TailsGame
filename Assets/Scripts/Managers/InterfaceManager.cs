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

public class InterfaceManager : MonoBehaviour
{
    internal static InterfaceManager _self;
    private void SingletonLogic()
    {
        if (_self != null)
        {
            Destroy(this);
        }
        else
        {
            _self = this;
        }
    }
    private void Awake()
    {
        SingletonLogic();
    }

    //private void print(string text)
    //{
    //    Debug.Log(text);
    //}

    //public static void PrintTextInConsole(string text)
    //{
    //    _self.print(text);
    //}





}

