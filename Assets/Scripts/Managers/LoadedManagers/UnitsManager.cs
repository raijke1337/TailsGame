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
        public SkillRequestedEvent UnitRequestsToPlaceASkillEvent;

        [SerializeField] private List<BaseUnit> _allUnits = new List<BaseUnit>();

        private EffectsManager _effects;

        private bool _paused;
        public bool GameplayPaused
        {
            get => _paused;
            set
            {
                _player.GetInputs<InputsPlayer>().IsInputsLocked = value;
                _paused = value;
            }
        }

        public override void Initiate()
        {
            _player = FindObjectOfType<PlayerUnit>();
            if (_player == null)
            {
                Debug.Log($"No player found in scene {this}");
                return;
            }
            if (GameManager.Instance.GetCurrentLevelData.Type != LevelType.Game) return; // used only for player overrides

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

            _effects = EffectsManager.Instance;
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
                u.SkillRequestSuccessEvent += (id, user, where) => UnitRequestsToPlaceASkillEvent?.Invoke(id, user, where);
                u.UnitRequestsSound += U_SoundPlayEvent;
                u.UnitRequestsParticles += U_UnitRequestsParticles;
                u.InitiateUnit();
            }
            else
            {
                u.BaseUnitDiedEvent -= (t) => HandleUnitDeath(t);
                u.SkillRequestSuccessEvent -= (id, user, where) => UnitRequestsToPlaceASkillEvent?.Invoke(id, user, where);
                u.UnitRequestsSound -= U_SoundPlayEvent;
                u.UnitRequestsParticles -= U_UnitRequestsParticles;
                u.DisableUnit();
            }
        }

        private void U_UnitRequestsParticles(CartoonFX.CFXR_Effect arg1, Transform arg2)
        {
            _effects.PlaceParticle(arg1, arg2);
        }

        private void U_SoundPlayEvent(AudioClip c, Vector3 where)
        {
            _effects.PlaySound(c, where);
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
        }

        #region scene

        // used by gamemanager to draw equips in menu and gallery

        // not necessary with inventory component
        //public void EquipVisualItemOnUnit(BaseUnit unit, string itemID)
        //{
        //    if (unit is PlayerUnit p)
        //    {
        //        p.DrawItem(itemID);
        //    }
        //}
        //public void EquipVisualItemOnUnit(BaseUnit unit, string[] itemID)
        //{
        //    if (unit is PlayerUnit p)
        //    {
        //        p.DrawItem(itemID);
        //    }
        //}

        #endregion

    }

}