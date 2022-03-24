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


public class SkillsManager : MonoBehaviour
{
    [SerializeField] private List<BaseSkill> _skills;
    private void Start()
    {
        _skills = new List<BaseSkill>();
        //_skills.AddRange(Extensions.GetAssetsFromPath<BaseSkill>(Constants.c_SkillClassesPath));
        //Debug.Log($"Found {_skills.Count()} skills");
    }

}

