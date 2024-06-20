using Arcatech.AI;
using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Arcatech.Managers
{
    public class UnitsManager : LoadedManagerBase
    {

        private PlayerUnit _player;
        public PlayerUnit GetPlayerUnit { get => _player; }
        private List<RoomUnitsGroup> _npcGroups;


        private EffectsManager _effects;
        private SkillsPlacerManager _skills;
        private TriggersManager _trigger;

        private bool _isUnitsLocked;
        public bool UnitsLocked
        {
            get => _isUnitsLocked;
            set
            {
                _player.LockUnit = value;
                if (_npcGroups != null)
                {
                    foreach (var g in _npcGroups)
                    {
                        foreach (var n in g.GetAllUnits)
                        {
                            n.LockUnit = value;
                        }
                    }
                }

                _isUnitsLocked = value;
            }
        }



        #region managed
        public override void Initiate()
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
            if (_player == null)
            {
                if (ShowDebug) Debug.Log($"No player found in scene {this}");
            }
            else
            {
                SetupUnit(_player, true);
            }

            var _rooms = FindObjectsOfType<EnemiesLevelBlockDecorComponent>();
            if (_rooms.Length == 0)
            {
                if (ShowDebug) Debug.LogWarning($"No rooms with enemies found");
            }
            else
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

        }
        public override void RunUpdate(float delta)
        {
            if (_isUnitsLocked) return;

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

        public override void Stop()
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


        private void SetupUnit(BaseUnit u, bool isEnable)
        {
            if (isEnable)
            {
                u.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
                
                u.SkillRequestFromInputsSuccessEvent += (id, user, where) => ForwardSkillRequests(id, user, where);
                u.BaseControllerEffectEvent += (t) => ForwardEffectsRequest(t,u);
                u.UnitTriggerRequestEvent += ForwardTriggerRequest;
                u.UnitPlacedProjectileEvent += ForwardProjectileRequest;


                u.InitiateUnit();
            }
            else
            {
                u.BaseUnitDiedEvent -= (t) => HandleUnitDeath(t);

                u.SkillRequestFromInputsSuccessEvent -= (id, user, where) => ForwardSkillRequests(id, user, where);
                u.BaseControllerEffectEvent -= (t) => ForwardEffectsRequest(t,u);
                u.UnitTriggerRequestEvent -= ForwardTriggerRequest;
                u.UnitPlacedProjectileEvent -= ForwardProjectileRequest;


                u.DisableUnit();
            }
        }


        private void HandleUnitDeath(BaseUnit unit)
        {
            if (ShowDebug) Debug.Log($"{unit} died");
                unit.DisableUnit();
            unit.LockUnit = true;
            
            if (unit is PlayerUnit)
            {
                UnitsLocked = true;
                GameManager.Instance.OnPlayerDead();
            }

        }

        #endregion

        #region unit requests
        private void ForwardProjectileRequest(ProjectileComponent arg, BaseUnit owner)
        {
           // Debug.Log($"{owner} placed projectile {arg}");
            _trigger.RegisterExistingProjectile(arg);
        }

        private void ForwardTriggerRequest(BaseUnit target, BaseUnit source, bool isEnter, TriggeredEffect cfg)
        {
            //Debug.Log($"{source.GetFullName} trigger request {cfg.name}");
            _trigger.ServeTriggerApplication(cfg, source, target, isEnter);
        }

        private void ForwardEffectsRequest(EffectRequestPackage pack,BaseUnit owner)
        {
            //Debug.Log($"{owner.GetFullName} sent a request to place effects: {pack}");
            _effects.ServeEffectsRequest(pack);
        }
        private void ForwardSkillRequests(SkillProjectileComponent cfg, BaseUnit user, Transform place)
        {
           // Debug.Log($"{user.GetFullName} skill request {cfg.name}");
            _skills.ServeSkillRequest(cfg, user, place);
        }

        #endregion

    }

}