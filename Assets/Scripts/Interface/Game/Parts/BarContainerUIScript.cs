using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class BarContainerUIScript : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private Image _bar;
        [SerializeField] private TextMeshProUGUI _text;

        private float _curr;
        private float _max;

        public void SetMax (float v)
        {
            _max = v;   
        }
        public void NewValue(float val)
        {
            _text.text = val.ToString();
            _curr = val;

            _bar.fillAmount = _curr / _max;
        }

        private void Awake()
        {
            if (_bg == null || _bar == null || _text == null)
            {
                Debug.LogError($"Not serialized properties in {gameObject}");
            }    
        }



    }
}