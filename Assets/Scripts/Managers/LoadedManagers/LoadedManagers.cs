using ModestTree;
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

        //Debug.Log($"Starting level manangers with leveldata: " + lv.LevelID);

        // if leveltype is scene only limited initiation for display
        if (_lvl.Type == LevelType.Game)
        {
            SkillsPlacerManager = GetComponent<SkillsPlacerManager>();
            TriggersProjectilesManager = GetComponent<TriggersProjectilesManager>();
            EventTriggersManager = GetComponent<EventTriggersManager>();
            StatsUpdatesHandler = GetComponent<StatsUpdatesHandler>();
            UnitsManager = GetComponent<UnitsManager>();
            GameInterfaceManager = Instantiate(GameManager.Instance.GetGameInterfacePrefab);


            _managers = new LoadedManagerBase[6];

            _managers[0] = TriggersProjectilesManager;
            _managers[1] = EventTriggersManager;
            _managers[2] = StatsUpdatesHandler;
            _managers[3] = UnitsManager;
            _managers[4] = SkillsPlacerManager;
            _managers[5] = GameInterfaceManager;
        }
        if (_lvl.Type == LevelType.Scene)
        {
            EventTriggersManager = GetComponent<EventTriggersManager>();
            GameInterfaceManager = Instantiate(GameManager.Instance.GetGameInterfacePrefab);
            _managers = new LoadedManagerBase[2] { EventTriggersManager,GameInterfaceManager };
        }
        foreach (var m in _managers)
        {

            if (m != null)
            {
                m.Initiate();
            }
            else
            {
                Debug.Log($"Null manager at index {_managers.IndexOf(m)}");
            }
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
        if (_lvl.Type == LevelType.Game || _lvl.Type == LevelType.Scene)
        {
            foreach (var m in _managers)
            { if (m != null) m.Stop(); }
        }
    }


}
