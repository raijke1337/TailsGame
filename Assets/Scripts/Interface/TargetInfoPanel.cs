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

public class TargetInfoPanel : InterfaceComp
{
    [SerializeField]
    private Text _nameT;
    [SerializeField]
    private Slider _hpBar;
    //[SerializeField]
    //public BaseStats LoadedStats {get;set;}

    //private void Start()
    //{
    //    if (LoadedStats== null)
    //    {
    //        ToggleActiveState();
    //    }
    //}

    //private void Update()
    //{
    //    if (LoadedStats is UnitStats s)
    //    {
    //        _hpBar.enabled = true;
    //        _nameT.text = s.Name;
    //        _hpBar.maxValue = s.maxHP;
    //        _hpBar.value = s.currHP;
    //    }
    //}
}
