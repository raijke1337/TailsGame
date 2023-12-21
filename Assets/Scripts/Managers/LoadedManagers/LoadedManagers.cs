
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Managers
{
    public class LoadedManagers : MonoBehaviour
    {
        public SkillsPlacerManager SkillsPlacerManager { get; private set; }
        public TriggersManager TriggersProjectilesManager { get; private set; }
        public EventTriggersManager EventTriggersManager { get; private set; }
        public UnitsManager UnitsManager { get; private set; }
        public GameInterfaceManager GameInterfaceManager { get; private set; }
        public IsoCameraController CameraController { get; private set; }








        private LoadedManagerBase[] _managers;

        private LevelData _lvl;

        public void Initiate(LevelData lv)
        {
            _lvl = lv;


            EventTriggersManager = GetComponent<EventTriggersManager>(); // for texts and event triggers

            UnitsManager = GetComponent<UnitsManager>();

            GameInterfaceManager = Instantiate(GameManager.Instance.GetGameInterfacePrefab);

            if (_lvl.Type == LevelType.Game)
            {
                SkillsPlacerManager = GetComponent<SkillsPlacerManager>();
                TriggersProjectilesManager = GetComponent<TriggersManager>();
                CameraController = Instantiate(GameManager.Instance.GetGameCameraPrefab);

                _managers = new LoadedManagerBase[6];

                _managers[0] = TriggersProjectilesManager;
                _managers[1] = EventTriggersManager;
                _managers[2] = UnitsManager;
                _managers[3] = SkillsPlacerManager;
                _managers[4] = GameInterfaceManager;
                _managers[5] = CameraController;
            }
            else
            {
                _managers = new LoadedManagerBase[3] { EventTriggersManager, UnitsManager, GameInterfaceManager };
            }
            foreach (var m in _managers)
            {
                Assert.IsNotNull(m);

                m.Initiate();
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
}