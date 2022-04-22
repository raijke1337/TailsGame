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

public class ServiceGolemInputs : InputsNPC
{
    [SerializeField] private DodgeController _dodgeCtrl;
    public void SetDodgeCtrl(string ID) => _dodgeCtrl = new DodgeController(ID);

}

