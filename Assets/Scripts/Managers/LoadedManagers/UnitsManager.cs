using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using Arcatech.Units.Inputs;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Managers
{
    public class UnitsManager : LoadedManagerBase
    {
        private List<RoomController> _rooms = new List<RoomController>();

        private List<NPCUnit> _npcs = new List<NPCUnit>();
        private PlayerUnit _player;
        public PlayerUnit GetPlayerUnit { get => _player; }
        public List<NPCUnit> GetNPCs() => _npcs;
        [SerializeField] private List<BaseUnit> _allUnits = new List<BaseUnit>();

        private EffectsManager _effects;
        private SkillsPlacerManager _skills;
        private TriggersManager _trigger;

        private bool _paused;
        public bool GameplayPaused
        {
            get => _paused;
            set
            {
                _player.LockUnit = value;
                foreach (var unit in _allUnits)
                {
                    unit.LockUnit = value;
                }
                _paused = value;
            }
        }

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
                Debug.Log($"No player found in scene {this}");
                return;
            }
           // if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return; // commented because I want unit mvmnt in scenes through their behavior system
           
           // and units won't init if they aren't in a room
           // used only for player actions overrides ie scene, intro etc
           // it breaks the visual item display in equips menu however.

            _allUnits.Add(_player);
            FinalUnitInit(_player, true);


            _rooms.AddRange(FindObjectsOfType<RoomController>());
            if (_rooms.Count == 0)
            {
                Debug.LogWarning($"No rooms found! Create some collider boxes with Room Controller");
            }

            foreach (var room in _rooms)
            {
                room.UnitFound += AddNPCToList;
                room.Initiate();
            }

        }

        private void AddNPCToList(NPCUnit n)
        {
            _npcs.Add(n);
            _allUnits.Add(n);
            FinalUnitInit(n, true);
            SetAIStateUnit(true, n);
        }


        public override void RunUpdate(float delta)
        {
            if (_paused) return;

            foreach (var u in _allUnits)
            {
                u.RunUpdate(delta);
            }

            foreach (var r in _rooms)
            {
                r.RunUpdate(delta);
            }
        }

        public override void Stop()
        {
            foreach (var r in _rooms)
            {
                r.UnitFound -= AddNPCToList;
                r.Stop();
            }
            foreach (var u in _allUnits)
            {
                FinalUnitInit(u, false);
            }
        }


        private void FinalUnitInit(BaseUnit u, bool isEnable)
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



        private void SetAIStateUnit(bool isProcessing, NPCUnit unit)
        {
            unit.AiToggle(isProcessing);
        }

        private void HandleUnitDeath(BaseUnit unit)
        {
            unit.DisableUnit();
            if (unit is NPCUnit)
            {
                SetAIStateUnit(false, unit as NPCUnit);
            }
            else
            {
                foreach (var n in _npcs)
                {
                    SetAIStateUnit(false, n); // to prevent activities (attacking the air etc)
                }
                GameManager.Instance.OnPlayerDead();
            }

            //Destroy(unit.gameObject, Constants.Combat.c_RemainsDisappearTimer);
        }
    }

}