using Arcatech.Items;
using Arcatech.Skills;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.UI
{
    public class IconContainersManager : ManagedControllerBase
    {
        
        [SerializeField] private IconContainerUIScript _iconPrefab;

        private Dictionary<SkillObjectForControls,IconContainerUIScript> _dict;

        public void TrackSkillIcon(SkillObjectForControls skill )
        {
            var icon = Instantiate(_iconPrefab, transform);
            _dict ??= new Dictionary<SkillObjectForControls, IconContainerUIScript>();
            _dict[skill] = icon;
            icon.Image.sprite = skill.Description.Picture;
        }

        public void UntrackSkillIcon(SkillObjectForControls o)
        {
            if (_dict != null && _dict.TryGetValue(o,out var icon))
            {
                Destroy(_dict[o]);
                _dict.Remove(o);
            }             
        }


        public override void StartController()
        {
            if (_iconPrefab == null)
            {
                Debug.LogError($"Set icon prefab in {this}!");
            }
            if (_dict == null)
            {
                _dict = new Dictionary<SkillObjectForControls, IconContainerUIScript>();
            }
        }


        public override void UpdateController(float delta)
        {
            
            foreach (var item in _dict.Keys)
            {
                _dict[item].Text = Mathf.RoundToInt(item.CurrentCooldown).ToString();
            }
        }

        public override void StopController()
        {
            
        }
    }
}