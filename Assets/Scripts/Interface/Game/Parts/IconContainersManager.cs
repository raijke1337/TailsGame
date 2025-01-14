using Arcatech.Items;
using Arcatech.Skills;
//using com.cyborgAssets.inspectorButtonPro;
using KBCore.Refs;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.UI
{
    public class IconContainersManager : MonoBehaviour
    {

        [SerializeField] private IconContainerUIScript _iconPrefab;
        [SerializeField, Space] private Transform _weaponsP;
        [SerializeField, Space] private Transform _skillsP;
        [SerializeField, Space] private Transform _othersP;

        private Dictionary<IIconContent, IconContainerUIScript> _weaponIcons = new();
        private Dictionary<IIconContent, IconContainerUIScript> _skillIcons = new();
        private Dictionary<IIconContent, IconContainerUIScript> _otherIcons = new();

        public void TrackIcon(IIconContent content)
        {

            if (content is not ISkill && content is not IWeapon)
            {
                if (_otherIcons.ContainsKey(content))
                {
                    _otherIcons[content].UpdateIcon(content);
                }

                else
                {
                    _otherIcons[content] = Instantiate(_iconPrefab, _othersP);
                    _otherIcons[content].UpdateIcon(content);
                }
            }
            //if (content is IWeapon weapon)
            //{
            //    if (_weaponIcons.ContainsKey(weapon))
            //    {
            //        _weaponIcons[weapon].UpdateIcon(weapon);
            //    }

            //    else
            //    {
            //        _weaponIcons[weapon] = Instantiate(_iconPrefab, _weaponsP);
            //        _weaponIcons[weapon].UpdateIcon(weapon);
            //    }
            //    return;
            //}
            else
            {
                if (_skillIcons.ContainsKey(content))
                {
                    _skillIcons[content].UpdateIcon(content);
                }

                else
                {
                    _skillIcons[content] = Instantiate(_iconPrefab, _skillsP);
                    _skillIcons[content].UpdateIcon(content);
                }
                return;
            }

        }

    }
}