using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using UnityEngine;

public class SkillsPlacerManager : LoadedManagerBase
{
    SerializableDictionaryBase<string, BaseSkill> _skillsDict = new SerializableDictionaryBase<string, BaseSkill>();
    public event SimpleEventsHandler<IProjectile> ProjectileSkillCreatedEvent;
    public event SimpleEventsHandler<IAppliesTriggers> SkillAreaPlacedEvent;

    #region ManagerBase
    public override void Initiate()
    {
        GameManager.Instance.GetGameControllers.UnitsManager.RequestToPlaceSkills += PlaceSkill;
        LoadBaseSkills();
        LoadDatasIntoSkills();
    }

    public override void RunUpdate(float delta)
    {

    }

    public override void Stop()
    {
        GameManager.Instance.GetGameControllers.UnitsManager.RequestToPlaceSkills -= PlaceSkill;
    }

    #endregion


    private void PlaceSkill(string ID, BaseUnit source, Transform empty)
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
        var skills = DataManager.Instance.GetAssetsOfType<BaseSkill>(Constants.PrefabsPaths.c_SkillPrefabs);
        foreach (var skill in skills)
        {
            _skillsDict[skill.SkillID] = skill;
        }
    }
    private void LoadDatasIntoSkills()
    {
        var cfgs = DataManager.Instance.GetAssetsOfType<SkillControllerDataConfig>(Constants.Configs.c_AllConfigsPath);
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

