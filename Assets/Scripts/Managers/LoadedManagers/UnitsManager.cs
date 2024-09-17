using Arcatech.AI;
using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using Arcatech.Units.Inputs;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.CanvasScaler;

namespace Arcatech.Managers
{
    public class UnitsManager : MonoBehaviour, IManagedController
    {

        private PlayerUnit _player;
        public PlayerUnit GetPlayerUnit { get => _player; }
        private List<RoomUnitsGroup> _npcGroups;

        private EffectsManager _effects;
        private SkillsPlacerManager _skills;
        private TriggersManager _trigger;

        public void LockUnits(bool IsLock)
        {
            _player.LockUnit = IsLock;
            if (_npcGroups != null)
            {
                foreach (var g in _npcGroups)
                {
                    foreach (var n in g.GetAllUnits)
                    {
                        n.LockUnit = IsLock;
                    }
                }
            }
        }

        #region managed
        public virtual void StartController()
        {
            if (_effects == null)
            {
                _effects = EffectsManager.Instance;
            }
            if (_trigger == null)
            {
                _trigger = GameManager.Instance.GetGameControllers.TriggersProjectilesManager;
            }
            if (_skills == null)
            {
                _skills = GameManager.Instance.GetGameControllers.SkillsPlacerManager;
            }

            _player = FindObjectOfType<PlayerUnit>();
            if (_player != null)
            { 
                SetupUnit(_player, true);
            }

            var _rooms = FindObjectsOfType<EnemiesLevelBlockDecorComponent>();
            if (_rooms.Length != 0) // rooms are set up
            {
                _npcGroups = new List<RoomUnitsGroup>();
                foreach (var room in _rooms)
                {
                    var g = new RoomUnitsGroup(room.GetAllUnitsInRoom);
                    _npcGroups.Add(g);
                    g.SpawnRoom = room;

                    foreach (var unit in g.GetAllUnits)
                    {
                        SetupUnit(unit, true);
                    }
                }
            }
            else // just units
            {
                foreach (DummyUnit u in FindObjectsOfType<DummyUnit>())
                {
                    if (!(u is PlayerUnit))
                    SetupUnit(u, true);
                }
            }

        }
        public virtual void ControllerUpdate(float delta)
        {
            if (_player.LockUnit) return;

            _player.RunUpdate(delta);
            if (_npcGroups == null) return;
            foreach (var g in _npcGroups)
            {
                foreach (var n in g.GetAllUnits.ToArray())
                {
                    n.RunUpdate(delta);
                }
            }
        }

        public virtual void FixedControllerUpdate(float fixedDelta)
        {
            if (_player.LockUnit) return;

            _player.RunFixedUpdate(fixedDelta);
            if (_npcGroups == null) return;
            foreach (var g in _npcGroups)
            {
                foreach (var n in g.GetAllUnits.ToArray())
                {
                    n.RunFixedUpdate(fixedDelta);
                }
            }
        }

        public virtual void StopController()
        {

            _player.DisableUnit();
            if (_npcGroups != null)
            {
                foreach (var g in _npcGroups)
                {
                    foreach (var n in g.GetAllUnits)
                    {
                        n.DisableUnit();
                    }
                }
            }

        }
        #endregion

        #region units handling


        private void SetupUnit(DummyUnit u, bool isEnable)
        {
            if (isEnable)
            {
                u.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
                u.StartControllerUnit();
            }
            else
            {
                u.BaseUnitDiedEvent -= (t) => HandleUnitDeath(t);
                u.DisableUnit();
            }
        }


        private void HandleUnitDeath(BaseUnit unit)
        {
                unit.DisableUnit();
            unit.LockUnit = true;
            
            if (unit is PlayerUnit)
            {
                LockUnits(true);
                GameManager.Instance.OnPlayerDead();
            }
            Destroy(unit.gameObject,2f);

        }
        #endregion

    }

}