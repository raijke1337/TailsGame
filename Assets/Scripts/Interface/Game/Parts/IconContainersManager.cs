using Arcatech.Items;
using Arcatech.Skills;
using com.cyborgAssets.inspectorButtonPro;
using KBCore.Refs;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.UI
{
    public class IconContainersManager : ValidatedMonoBehaviour
    {

        [SerializeField] private IconContainerUIScript _iconPrefab;
        private Dictionary<IIconContent, IconContainerUIScript> _icons = new();

        public void TrackIcon(IIconContent content)
        {
            if (_icons.ContainsKey(content))
            {
                _icons[content].UpdateIcon(content);
            }

            else
            {
                _icons[content] = Instantiate(_iconPrefab, this.transform);
                _icons[content].UpdateIcon(content);
            }
        }


#if UNITY_EDITOR
        [Space,SerializeField] WeaponSO loadedIcon;
        [ProButton]
        public void Debug_LoadIcon()
        {
            Weapon w = new Weapon(loadedIcon, null);
            TrackIcon(w);
        }
#endif

    }
}