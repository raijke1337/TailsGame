using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public abstract class BaseController : IManagedController
    {
        public EquippedUnit Owner { get; protected set; }

        public BaseController(EquippedUnit owner)
        {
            Owner = owner;
        }



        public abstract void ApplyEffect(StatsEffect effect);

        #region managed
        public abstract void StartController();
        public abstract void ControllerUpdate(float delta);
        public abstract void FixedControllerUpdate(float fixedDelta);
        public abstract void StopController();

        #endregion



    }

}