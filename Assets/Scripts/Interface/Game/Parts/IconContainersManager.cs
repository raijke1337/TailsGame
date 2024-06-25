using Arcatech.Skills;
using KBCore.Refs;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.UI
{
    public class IconContainersManager : ValidatedMonoBehaviour
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


    }
}