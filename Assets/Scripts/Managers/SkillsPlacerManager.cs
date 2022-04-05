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
using RotaryHeart.Lib.SerializableDictionary;

public class SkillsPlacerManager : MonoBehaviour
{
    [SerializeField] SerializableDictionaryBase<string,BaseSkill> _skillsDict;
    // todo load from folder

    private List<BaseSkill> _placedSkills = new List<BaseSkill>(); // todo not used 

    private UnitsManager _unitsM;

    private void OnEnable()
    {
        _unitsM = GetComponent<UnitsManager>();
        _unitsM.RequestToPlaceSkills += PlaceSkill;
    }
    private void OnDisable()
    {
        _unitsM.RequestToPlaceSkills -= PlaceSkill;
    }

    private void PlaceSkill(string ID, BaseUnit source)
    {
        Debug.Log($"{source.GetFullName()} requested skill ID {ID}");
        var skill = Instantiate(_skillsDict[ID]);
        skill.Source = source;
    }
}

