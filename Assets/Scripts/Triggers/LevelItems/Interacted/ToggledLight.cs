using KBCore.Refs;
using UnityEngine;

namespace Arcatech.Level
{
    [RequireComponent(typeof(Light))]
    public class ToggledLight : ConditionControlledItem
    {
        [SerializeField] Color negColor = Color.red;
        [SerializeField] Color posColor = Color.green;
        Light _light;

        private void Start()
        {
            _light = GetComponent<Light>();
            _light.color = negColor;
        }
        protected override void OnSetState(bool newstate)
        {
            if (newstate) _light.color = posColor;
            else _light.color = negColor;
        }
    }

}