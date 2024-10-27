using Arcatech.Puzzles;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Arcatech.Level;
using Arcatech.EventBus;
using Arcatech.Items;

namespace Arcatech.Managers
{
    public class LevelManager : MonoBehaviour, IManagedController
    {

        protected List<LevelBlockDecorPrefabComponent> _levelBlocks;

        #region managed
        public virtual void StartController()
        {

            _levelBlocks = FindObjectsOfType<LevelBlockDecorPrefabComponent>().ToList(); // find only starting "cubes"
            foreach (var t in _levelBlocks)
            {
                t.StartController();
            }

        }

        public virtual void ControllerUpdate(float delta)
        {
            foreach (var t in _levelBlocks)
            {
                t.ControllerUpdate(delta);
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



        #endregion


    }
}