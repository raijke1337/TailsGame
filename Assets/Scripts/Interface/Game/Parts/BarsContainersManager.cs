using Arcatech.Stats;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.UI
{
    public class BarsContainersManager : ManagedControllerBase
    {
        [SerializeField] SerializedDictionary<DisplayValueType, BarContainerUIScript> _barsDict;


        public void LoadValues(StatValueContainer cont, DisplayValueType type)
        {
            return;
            Assert.IsNotNull(cont, $"Loading null into {this}");
            switch (type)
            {
                default:
                    _barsDict[type].Container = cont;
                    _barsDict[type].gameObject.SetActive(true);
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
            foreach (var bar in _barsDict.Values)
            {
                bar.gameObject.SetActive(false);
            }
        }
    }
}