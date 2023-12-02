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
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _text;

        private StatValueContainer _valueContainer;
        private float _tgtFillValue = 1;


        public StatValueContainer Container 
        { 
            get
            {
                return _valueContainer;
            }
                
            set
            {
                if (_valueContainer != value)
                {
                    if (_valueContainer != null)
                    {
                        _valueContainer.ValueChangedEvent -= OnUpdatedValue;
                    }
                    _fill.fillAmount = _tgtFillValue;

                    _valueContainer = value;
                    _valueContainer.ValueChangedEvent += OnUpdatedValue;

                    _text.text = value.ToString();
                }
            }                
        }



        private void OnUpdatedValue(float old, float now)
        {
            _text.text = Container.ToString();
            _tgtFillValue = Container.GetCurrent / Container.GetMax;
        }

        public void UpdateValues(float delta)
        {
            if (_tgtFillValue == _fill.fillAmount) return;
            else
            {
                float vel = 0;
                _fill.fillAmount = Mathf.SmoothDamp(_fill.fillAmount, _tgtFillValue, ref vel, delta);

               // Debug.Log($"{this} ref velocity : {vel}");
            }
        }




    }
}