using Arcatech.Texts;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class LevelTextTrigger : BaseLevelEventTrigger
    {
        [SerializeField] private DialoguePart _text;
        public DialoguePart Text
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


    }
}