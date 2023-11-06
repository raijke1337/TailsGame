using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.UI
{
    public class BarsContainersManager : ManagedControllerBase
    {
        private Dictionary<StatValueContainer, BarContainerUIScript> _bars;

        [SerializeField] BarContainerUIScript _barPrefab;

        public void ProcessContainer(StatValueContainer cont, bool isAdding)
        {
            if (isAdding)
            {
                if (!_bars.ContainsKey(cont))
                {
                    _bars[cont] = Instantiate(_barPrefab, transform);
                    cont.ValueChangedEvent += (cur, prev) => _bars[cont].NewValue(cur);
                    var b = _bars[cont];
                    b.SetMax(cont.GetMax);
                    b.NewValue(cont.GetCurrent);                    
                }
                else
                {
                    Debug.LogWarning($"Loading already added container into {gameObject}");
                }
            }
            if (!isAdding)
            {
                var go = _bars[cont];
                cont.ValueChangedEvent -= (cur, prev) => _bars[cont].NewValue(cur);
                _bars.Remove(cont);
                Destroy(go); // remove the bar from panel
            }
        }
        public void LoadContainers(IEnumerable<StatValueContainer> conts)
        {
            foreach (StatValueContainer cont in conts) ProcessContainer(cont,true);
        }

        public override void StartController()
        {
            _bars =  new Dictionary<StatValueContainer, BarContainerUIScript>();
            if (_barPrefab == null) Debug.LogError($"Set bar prefab in {this}!");
        }

        public override void UpdateController(float delta)
        {
            
        }

        public override void StopController()
        {
            if (_bars != null || _bars.Count == 0) ;
            foreach (var cont in _bars.Keys)
            {
                cont.ValueChangedEvent -= (cur, prev) => _bars[cont].NewValue(cur);
            }
        }
    }
}