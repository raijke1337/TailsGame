using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class LevelTextTrigger : BaseLevelEventTrigger
    {
        [SerializeField] private TextContainerSO _text;
        public TextContainerSO Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        protected override void Start()
        {
            base.Start();
            Text = _text;
        }
    }
}