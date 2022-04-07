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

    private UnitsManager _unitsM;
    public event SimpleEventsHandler<IProjectile,string> ProjectileSkillCreatedEvent;

    private Dictionary<string,SkillData> _datas = new Dictionary<string,SkillData>();

    private void OnEnable()
    {
        _unitsM = GetComponent<UnitsManager>();
        _unitsM.RequestToPlaceSkills += PlaceSkill;

        var cfgs = Extensions.GetAssetsFromPath<SkillControllerDataConfig>(Constants.Configs.c_SkillConfigsPath);
        foreach (var cfg in cfgs)
        {
            _datas.Add(cfg.ID,cfg.Data);
        }
    }
    private void OnDisable()
    {
        _unitsM.RequestToPlaceSkills -= PlaceSkill;
    }

    private void PlaceSkill(string ID, BaseUnit source)
    {
        var skill = Instantiate(_skillsDict[ID]);
        skill.Source = source;

        skill.SkillData = new SkillData(_datas[ID]);

        skill.transform.SetPositionAndRotation(source.transform.position, source.transform.rotation);
        skill.transform.forward = source.transform.forward;


        if (skill is IProjectile)
        {
            var sk = skill as IProjectile;
            ProjectileSkillCreatedEvent?.Invoke(sk, ID);            
        }
    }
}

