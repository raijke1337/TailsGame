using Arcatech.Texts;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class LevelTextTrigger : BaseLevelEventTrigger
    {
        [SerializeField] private ExtendedTextContainerSO _text;
        public ExtendedTextContainerSO Text
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

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }

    }
}