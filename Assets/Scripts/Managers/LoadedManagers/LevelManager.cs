using Arcatech.Puzzles;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Arcatech.Level;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Effects;

namespace Arcatech.Managers
{
    public class LevelManager : MonoBehaviour, IManagedController
    {

        protected List<LevelBlockDecorPrefabComponent> _levelBlocks;

        #region managed
        public virtual void StartController()
        {
            if (_passiveEvents != null)
            {
                _passiveEvents.Clear();
            }
            else
            {
                _passiveEvents = new List<PassiveEventTrigger> ();
            }
            _passiveEvents.AddRange(FindObjectsOfType<PassiveEventTrigger>());

            _levelBlocks = FindObjectsOfType<LevelBlockDecorPrefabComponent>().ToList(); // find only starting "cubes"
            foreach (var t in _levelBlocks)
            {
                t.StartController();
            }
            var music = GameManager.Instance.GetCurrentLevelData.Music;

        }

        public virtual void ControllerUpdate(float delta)
        {
            foreach (var t in _levelBlocks)
            {
                t.ControllerUpdate(delta);
            }
            foreach (var t in _passiveEvents.ToList())
            {
                if (!t.Triggered)
                {
                    t.CheckEventTrigger();
                }
                else
                {
                    _passiveEvents.Remove(t);
                }
            }    
        }
        public virtual void FixedControllerUpdate(float fixedDelta)
        {

        }

        public virtual void StopController()
        {
            foreach (var t in _levelBlocks)
            {
                t.StopController();
            }
        }
        #endregion

        #region level events

        [SerializeField] protected List<PassiveEventTrigger> _passiveEvents;


        #endregion


    }
}