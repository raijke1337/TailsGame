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
        //  private List<RoomUnitsGroup> _npcGroups;
        List<BaseEntity> entities = new List<BaseEntity>();


        private EffectsManager _effects;
        private SkillsPlacerManager _skills;
        private TriggersManager _trigger;

        public void LockUnits(bool IsLock)
        {
            _player.UnitPaused = IsLock;
            //if (_npcGroups != null)
            //{
            //    foreach (var g in _npcGroups)
            //    {
            //        foreach (var n in g.GetAllUnits)
            //        {
            //            n.UnitPaused = IsLock;
            //        }
            //    }
            //}
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
            //_npcGroups = new List<RoomUnitsGroup>();

            //var _rooms = FindObjectsOfType<EnemiesLevelBlockDecorComponent>();
            //if (_rooms.Length != 0) // rooms are set up
            //{
            //    foreach (var room in _rooms)
            //    {
            //        var g = new RoomUnitsGroup(room.GetAllUnitsInRoom);
            //        _npcGroups.Add(g);
            //        g.SpawnRoom = room;

            //        foreach (var unit in g.GetAllUnits)
            //        {
            //            SetupUnit(unit, true);
            //        }
            //    }
            //}
            //else // just units
            //{
            //    List<BaseEntity> l = new List<BaseEntity>();
            //    foreach (BaseEntity u in FindObjectsOfType<BaseEntity>())
            //    {
            //        if (!(u is PlayerUnit))
            //        SetupUnit(u, true);
            //        l.Add(u);
            //    }
            //   // _npcGroups.Add(new RoomUnitsGroup(u));
            //}

            List<BaseEntity> l = new List<BaseEntity>();
            foreach (BaseEntity u in FindObjectsOfType<BaseEntity>())
            {
                if (!(u is PlayerUnit))
                {
                    SetupUnit(u,true);
                    entities.Add(u);
                }
            }

        }
        public virtual void ControllerUpdate(float delta)
        {
            if (_player.UnitPaused) return;

            _player.RunUpdate(delta);
            foreach (var e in entities)
            {
                e.RunUpdate(delta);
            }
            //if (_npcGroups == null) return;
            //foreach (var g in _npcGroups)
            //{
            //    foreach (var n in g.GetAllUnits.ToArray())
            //    {
            //        n.RunUpdate(delta);
            //    }
            //}
        }

        public virtual void FixedControllerUpdate(float fixedDelta)
        {
            if (_player.UnitPaused) return;

            _player.RunFixedUpdate(fixedDelta);
            foreach (var e in entities)
            {
                e.RunFixedUpdate(fixedDelta);
            }

            //if (_npcGroups == null) return;
            //foreach (var g in _npcGroups)
            //{
            //    foreach (var n in g.GetAllUnits.ToArray())
            //    {
            //        n.RunFixedUpdate(fixedDelta);
            //    }
            //}
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

            //if (_npcGroups != null)
            //{
            //    foreach (var g in _npcGroups)
            //    {
            //        foreach (var n in g.GetAllUnits)
            //        {
            //            n.DisableUnit();
            //        }
            //    }
            //}

        }
        #endregion

        #region units handling


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
            unit.DisableUnit();
            
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