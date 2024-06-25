using Arcatech.EventBus;
using Arcatech.Stats;
using Arcatech.Texts;
using KBCore.Refs;
using UnityEngine;

namespace Arcatech.UI
{
    public class PlayerUnitPanel : ValidatedMonoBehaviour
    {
        [SerializeField,Child] protected IconContainersManager _icons;
        [SerializeField,Child] protected BarsContainersManager _bars;


        public void ShowStat (BaseStatType type , StatValueContainer cont) => _bars.UpdateBarValue(type, cont);
        public void ShowSkill()
        {
            Debug.Log($"NYI");
        }

    }

}