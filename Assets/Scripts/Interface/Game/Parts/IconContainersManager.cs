using Arcatech.Skills;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.UI
{
    public class IconContainersManager : ManagedControllerBase
    {

        [SerializeField] private IconContainerUIScript _iconPrefab;

        private List<IconContainerUIScript> _icons;

        public void TrackSkillIcon(SkillObjectForControls skill)
        {
            var icon = Instantiate(_iconPrefab, transform);
            _icons ??= new List<IconContainerUIScript>();
            _icons.Add(icon);
            icon.LoadedSkill = skill;
        }


        public override void StartController()
        {
            if (_iconPrefab == null)
            {
                Debug.LogError($"Set icon prefab in {this}!");
            }
        }


        public override void UpdateController(float delta)
        {

            foreach (var item in _icons)
            {
                item.UpdateInDelta(delta);
            }
        }

        public override void StopController()
        {

        }
    }
}