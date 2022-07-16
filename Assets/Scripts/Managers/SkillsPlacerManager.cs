using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using RotaryHeart.Lib.SerializableDictionary;

public class SkillsPlacerManager : MonoBehaviour
{
    SerializableDictionaryBase<string,BaseSkill> _skillsDict = new SerializableDictionaryBase<string, BaseSkill>();
    private UnitsManager _unitsM;
    public event SimpleEventsHandler<IProjectile> ProjectileSkillCreatedEvent;
    public event SimpleEventsHandler<IAppliesTriggers> SkillAreaPlacedEvent;


    public UnitsManager GetUnitsManager => _unitsM;

    private void OnEnable()
    {
        _unitsM = GetComponent<UnitsManager>();
        _unitsM.RequestToPlaceSkills += PlaceSkill;
        LoadBaseSkills();
        LoadDatasIntoSkills();
    }
    private void OnDisable()
    {
        _unitsM.RequestToPlaceSkills -= PlaceSkill;
    }

    private void PlaceSkill(string ID, BaseUnit source,Transform empty)
    {
        var skill = Instantiate(_skillsDict[ID]);
        skill.Owner = source;
        skill.transform.SetPositionAndRotation(empty.position, empty.rotation);
        if (skill is IProjectile)
        {
            var sk = skill as IProjectile;
            ProjectileSkillCreatedEvent?.Invoke(sk);
            // further handling by projectiles manager (expiry, movement)
        }
        else
        {
            SkillAreaPlacedEvent?.Invoke(skill);
            skill.HasExpiredEvent += HandleSkillExpiry;
        }
    }

    private void LoadBaseSkills()
    {
        var skills = Extensions.GetAssetsOfType<BaseSkill>(Constants.Combat.c_SkillPrefabs); 
        foreach (var skill in skills)
        {
            _skillsDict[skill.SkillID] = skill;
        }
    }
    private void LoadDatasIntoSkills()
    {
        var cfgs = Extensions.GetAssetsOfType<SkillControllerDataConfig>(Constants.Configs.c_AllConfigsPath);
        foreach (var skill in _skillsDict.Values)
        {
            var dataCfg = cfgs.First(t => t.ID == skill.SkillID);
            skill.SkillData = new SkillData(dataCfg.Data);
        }
    }

    private void HandleSkillExpiry(IExpires item)
    {
        item.HasExpiredEvent -= HandleSkillExpiry;
        Destroy(item.GetObject());
    }

#if UNITY_EDITOR
    [ContextMenu("Load skills and configs")]
    public void RefreshStuff()
    {
        LoadBaseSkills();
        LoadDatasIntoSkills();
    }

#endif

}

