
using Arcatech.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class IconContainerUIScript : MonoBehaviour
    {
        public SkillObjectForControls LoadedSkill { get => _skill; set
            {
#if UNITY_EDITOR
                Assert.IsNotNull(value);
                Assert.IsNotNull(_timerFill);
                Assert.IsNotNull(_icon);
                Assert.IsNotNull(_text);
#endif


            }
        }
        private SkillObjectForControls _skill;        
        [SerializeField] private Image[] _timerFill;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _text;



        public void UpdateInDelta(float delta)
        {

        }

    }
}