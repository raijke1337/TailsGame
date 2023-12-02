using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.UI
{
    public class BarsContainersManager : ManagedControllerBase
    {
        [SerializeField] private BarContainerUIScript _hpB;
        [SerializeField] private BarContainerUIScript _shB;
        [SerializeField] private BarContainerUIScript _combB;

        private List<BarContainerUIScript> _bars;



        public void ProcessContainer(StatValueContainer cont, DisplayValueType type)
        {
            switch (type)
            {
                case DisplayValueType.Health:
                    _hpB.Container = cont;
                    break;
                case DisplayValueType.Shield:
                    _shB.Container = cont;
                    break;
                case DisplayValueType.Combo:
                    _combB.Container = cont;
                    break;
                    default:
                    break;
            }
        }


        public override void UpdateController(float delta)
        {
            foreach (var c in _bars)
            {
                if (c != null)
                c.UpdateValues(delta);
            }
        }

        public override void StopController()
        {

        }

        public override void StartController()
        {
            _bars = new List<BarContainerUIScript>
            {
                _hpB,
                _shB,
                _combB
            };
        }
    }
}