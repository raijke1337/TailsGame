using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.UI
{
    public abstract class PanelWithBarGeneric : ManagedControllerBase
    {
        [SerializeField] protected BarsContainersManager _bars;
        [SerializeField, Space] protected float _barFillRateMult = 1f;

        protected bool _act;
        public virtual bool IsNeeded
        {
            get => _act;
            set
            {
                _act = value;
                gameObject.SetActive(_act);
            }
        }
        public override void UpdateController(float delta)
        {
            _bars.UpdateController(delta);
        }
        public override void StartController()
        {
            _bars.StartController();
        }

    }
}