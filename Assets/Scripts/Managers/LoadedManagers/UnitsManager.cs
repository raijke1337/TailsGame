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

            _pauseBind = new EventBinding<PauseToggleEvent>(OnPawsToggle);
            EventBus<PauseToggleEvent>.Register(_pauseBind);

        }
        public virtual void ControllerUpdate(float delta)
        {
            if (_player.UnitPaused) return;

            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].RunUpdate(delta);
            }

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

            EventBus<PauseToggleEvent>.Deregister(_pauseBind);
        }
        #endregion

        #region units handling

        private PlayerUnit _player;
        public PlayerUnit GetPlayerUnit
        {
            get
            {
                if (_player == null)
                {
                    _player = FindObjectOfType<PlayerUnit>();
                }
                return _player;

            }
        }
            //  private List<RoomUnitsGroup> _npcGroups;
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
               // GM_OnPauseToggle(true);
                GameManager.Instance.OnPlayerDead();
            }
            else
            {
                Destroy(unit.gameObject, 2f);
            }

        }
        #endregion

    }

}