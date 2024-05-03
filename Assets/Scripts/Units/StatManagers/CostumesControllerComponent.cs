using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    public class CostumesControllerComponent : ManagedControllerBase
    {
        [SerializeField] private List<SkinnedMeshRenderer> _parts;

        public void OnBreak()
        {
            if (_parts != null)
            {
                if (_parts.Count > 0)
                {
                    var ind = Random.Range(0, _parts.Count - 1);
                    var s = _parts[ind];

                    s.enabled = false;
                    _parts.RemoveAt(ind);
                }
            }
        }


        public override void StartController()
        {
            
        }

        public override void StopController()
        {
            
        }

        public override void UpdateController(float delta)
        {
            
        }
    }

}