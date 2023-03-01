using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedManagers : MonoBehaviour
{
    public SkillsPlacerManager SkillsPlacerManager;
    public TriggersProjectilesManager TriggersProjectilesManager;
    public EventTriggersManager EventTriggersManager;
    public StatsUpdatesHandler StatsUpdatesHandler;
    public UnitsManager UnitsManager;
    public GameInterfaceManager GameInterfaceManager;

    private LoadedManagerBase[] _managers;

    private LevelData _lvl;

    public void Initiate(LevelData lv)
    {
        _lvl = lv;
        if (_lvl.Type == LevelType.Game)
        {
            SkillsPlacerManager = GetComponent<SkillsPlacerManager>();
            TriggersProjectilesManager = GetComponent<TriggersProjectilesManager>();
            EventTriggersManager = GetComponent<EventTriggersManager>();
            StatsUpdatesHandler = GetComponent<StatsUpdatesHandler>();
            UnitsManager = GetComponent<UnitsManager>();
            GameInterfaceManager = FindObjectOfType<GameInterfaceManager>();
            // if leveltype is scene only limited initiation for display

            _managers = new LoadedManagerBase[6];

            _managers[0] = TriggersProjectilesManager;
            _managers[1] = EventTriggersManager;
            _managers[2] = StatsUpdatesHandler;
            _managers[3] = UnitsManager;
            _managers[4] = SkillsPlacerManager;
            _managers[5] = GameInterfaceManager;


            foreach (var m in _managers)
            {
                if (m != null)
                    m.Initiate();
                else
                {
                    Debug.Log($"Null manager");
                }
            }
        }
        else
        {
            Debug.Log($"Attempted to initiate Gameplay managers for level {_lvl.LevelID} type {_lvl.Type} and nothing happened");
        }

    }

    public void UpdateManagers(float delta)
    {
        if (_lvl.Type == LevelType.Game)
        {
            foreach (var m in _managers)
            {
                if (m != null)
                    m.RunUpdate(delta);
            }
        }

    }
    public void Stop()
    {
        if (_lvl.Type == LevelType.Game)
        {
            foreach (var m in _managers)
            { if (m != null) m.Stop(); }
        }
    }


}
