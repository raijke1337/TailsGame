using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.UI
{
    public class BarsContainersManager : ManagedControllerBase
    {
        [SerializeField] SerializedDictionary<DisplayValueType, BarContainerUIScript> _barsDict;


        public void LoadValues(StatValueContainer cont, DisplayValueType type)
        {
            Assert.IsNotNull(cont,$"Loading null into {this}");
            switch (type)
            {
                case DisplayValueType.Health:
                    _barsDict[type].Container = cont;
                    break;
                case DisplayValueType.Shield:
                    _barsDict[type].Container = cont;
                    break;
                case DisplayValueType.Combo:
                    _barsDict[type].Container = cont;
                    break;
                default:
                    break;
            }
        }

        public override void UpdateController(float delta)
        {
            foreach (var c in _barsDict.Values)
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

        }
    }
}