using Arcatech.Actions;
using Arcatech.EventBus;
using CartoonFX;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    public class CostumesControllerComponent : MonoBehaviour
    {

        public void Init(BaseEntity unit) => owner = unit;

        BaseEntity owner;

        [SerializeField] private List<GameObject> _parts;
        [SerializeField] SerializedActionResult[] BreakEffects;
        public void OnBreak()
        {
            if (_parts != null)
            {
                if (_parts.Count > 0)
                {
                    var ind = Random.Range(0, _parts.Count - 1);
                    var s = _parts[ind];


                    if (BreakEffects != null && BreakEffects.Length > 0)
                    {
                        foreach (var f in BreakEffects)
                        {
                            f.GetActionResult().ProduceResult(owner, null, owner.transform);
                        }
                    }
                    s.SetActive(false);
                    _parts.RemoveAt(ind);
                }
            }
        }

    }

}