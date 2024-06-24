
using UnityEngine;
namespace Arcatech.Triggers
{
    public class StatsMod
    {
        public StatsMod(SerializedStatModConfig cfg)
        {
            InitialValue = cfg.InitialValue;
            Icon = cfg.Icon;
            StatType = cfg.ChangedStat;
            PerSecondChange = cfg.OverTimeValue;
        }
        public StatsMod(BaseStatType statType, float initialValue, float overTimeValue)
        {
            InitialValue = initialValue;
            StatType = statType;
            PerSecondChange = overTimeValue;
        }

        public float InitialValue { get; set; }
        public Sprite Icon { get; }
        public BaseStatType StatType { get; }
        public float PerSecondChange { get; set; }
        public virtual bool CheckCondition => true; // TODO: implement predicates check for conditions

    }

}