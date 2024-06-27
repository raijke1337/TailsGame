
using Arcatech.Scenes;
using Arcatech.Scenes.Cameras;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Managers
{
    public class LoadedManagers : ManagedControllerBase
    {
        public SkillsPlacerManager SkillsPlacerManager { get; private set; }
        public TriggersManager TriggersProjectilesManager { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public UnitsManager UnitsManager { get; private set; }
        public GameInterfaceManager GameInterfaceManager { get; private set; }
        public IsoCameraController CameraController { get; private set; }


        private LoadedManagerBase[] _managers;
        LevelType _currentLvlType;
        // private SceneContainer _lvl;

        public void StartController(LevelType lv)
        {
            // _lvl = lv;
            _currentLvlType = lv;

             LevelManager = GetComponent<LevelManager>(); // for texts and event triggers

            UnitsManager = GetComponent<UnitsManager>();

            GameInterfaceManager = Instantiate(GameManager.Instance.GetGameInterfacePrefab);

            if (lv == LevelType.Game)
            {
                SkillsPlacerManager = GetComponent<SkillsPlacerManager>();
                TriggersProjectilesManager = GetComponent<TriggersManager>();

                CameraController = FindObjectOfType<IsoCameraController>();
                if (CameraController == null)
                {
                    CameraController = Instantiate(GameManager.Instance.GetGameCameraPrefab);
                }          

                _managers = new LoadedManagerBase[6];

                _managers[0] = LevelManager; // init level blocks 
                _managers[1] = TriggersProjectilesManager; // load spawned and pre-placed triggers
                _managers[2] = UnitsManager; // load spawned  units
                _managers[3] = SkillsPlacerManager;
                _managers[4] = GameInterfaceManager;
                _managers[5] = CameraController;
            }
            else
            {
                _managers = new LoadedManagerBase[3] { LevelManager, UnitsManager, GameInterfaceManager };
            }
            foreach (var m in _managers)
            {
                Assert.IsNotNull(m);

                m.StartController();
            }

        }


        public override void ControllerUpdate(float delta)
        {
            foreach (var m in _managers)
            {
                if (m != null)
                    m.ControllerUpdate(delta);
            }
        }

        public override void FixedControllerUpdate(float fixedDelta)
        {
            foreach (var m in _managers)
            {
                if (m != null)
                    m.FixedControllerUpdate(fixedDelta);
            }
        }

        public override void StopController()
        {
            if (_currentLvlType == LevelType.Game || _currentLvlType == LevelType.Scene)
            {
                foreach (var m in _managers)
                { if (m != null) m.StopController(); }
            }
        }

        public override void StartController()
        {
        }
    }
}