using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Level
{
    public class ConditionToggledItem : ConditionControlledItem
    {
        [SerializeField] protected List<GameObject> _items;
        protected override void OnSetState(bool newstate)
        {
            foreach (var item in _items)
            {
                item.SetActive(!newstate);
            }

        }
    }
}