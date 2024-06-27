
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
                _skill = value;


                _timerFill.fillAmount = 1;
                _timerFill.enabled = false;
                //_icon.sprite = _skill.Description.Picture;
                //_text.text = _skill.GetTextForUI;


            }
        }
        private SkillObjectForControls _skill;        
        [SerializeField] private Image _timerFill;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _text;



        public void UpdateInDelta(float delta)
        {

        }

    }


}