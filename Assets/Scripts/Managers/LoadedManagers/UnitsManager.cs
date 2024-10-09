using Arcatech.BlackboardSystem;
using Arcatech.EventBus;
using Arcatech.Units;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Arcatech.Managers
{
    public class UnitsManager : MonoBehaviour, IManagedController
    {

        #region pause handling

        EventBinding<PauseToggleEvent> _pauseBind;
        private void OnPawsToggle(PauseToggleEvent isPausing)
        {
            _player.UnitPaused = !_player.UnitPaused;
            Debug.Log($"Paws the game: {_player.UnitPaused}");
            foreach(var e in entities)
            {
                e.UnitPaused = _player.UnitPaused;
            }
        }
        #endregion

        public static UnitsManager Instance;
        private void Awake()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            Instance = this;
        }


        #region managed
        public virtual void StartController()
        {

            List<BaseEntity> l = new List<BaseEntity>();
            foreach (BaseEntity u in FindObjectsOfType<BaseEntity>())
            {
                SetupUnit(u, true);
                entities.Add(u);
                if (u is PlayerUnit p)
                {
                    _player = p;
                }
            }
            SetupBlackboard();
        }
        public virtual void ControllerUpdate(float delta)
        {
            if (_player.UnitPaused) return;

            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].RunUpdate(delta);
            }
            UpdateBlackboardController();
        }

        public virtual void FixedControllerUpdate(float fixedDelta)
        {
            if (_player.UnitPaused) return;

            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].RunFixedUpdate(fixedDelta);
            }

        }

        public virtual void StopController()
        {

            _player.DisableUnit();
            SetupUnit(_player, false);
            foreach (var e in entities)
            {
                e.DisableUnit();
                SetupUnit(e, false);
            }
            entities.Clear();
        }
        #endregion

        #region units handling

        private PlayerUnit _player;
        List<BaseEntity> entities = new List<BaseEntity>();

        private void SetupUnit(BaseEntity u, bool isEnable)
        {
            if (isEnable)
            {
                u.BaseEntityDeathEvent += (t) => HandleUnitDeath(t);
                u.StartControllerUnit();
            }
            else
            {
                u.BaseEntityDeathEvent -= (t) => HandleUnitDeath(t);
                u.DisableUnit();
            }
        }


        private void HandleUnitDeath(BaseEntity unit)
        {

            SetupUnit(unit, false);
            entities.Remove(unit);
            
            if (unit is PlayerUnit)
            {
                GameInterfaceManager.Instance.GameOver();
            }
            else
            {
                Destroy(unit.gameObject, 2f);
            }

        }
        #endregion

        private void OnEnable()
        {
            _pauseBind = new EventBinding<PauseToggleEvent>(OnPawsToggle);
            EventBus<PauseToggleEvent>.Register(_pauseBind);
        }
        private void OnDisable()
        {
            EventBus<PauseToggleEvent>.Deregister(_pauseBind);
        }


        #region blackboard


        [Header("Blackboard initial settings"), SerializeField] BlackboardData bbData;

        readonly Blackboard bb = new();
        readonly Arbiter ar = new Arbiter();
        BlackboardKey safeSpot;
        private void SetupBlackboard()
        {
            bbData.SetupBlackboard(bb);
            safeSpot = bb.GetOrRegisterKey("safeSpotLocation");

            foreach (var e in entities)
            {
                if (e is IExpert ex)
                {
                    ar.RegisterExpert(ex);
                }
            }       
        }
        public Blackboard GetBlackboard => bb;

        void UpdateBlackboardController()
        {
            foreach (var act in ar.BlackboardIteration(bb))
            {
                act();
            }
        }
        #endregion

    }

}