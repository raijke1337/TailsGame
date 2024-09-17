
using UnityEngine;
namespace Arcatech.Triggers
{
    public class StatsMod
    {
        public StatsMod(SerializedStatModConfig cfg)
        {
            InitialValue = cfg.InitialValue;
            StatType = cfg.ChangedStat;
            hash = cfg.Hash;
        }
        protected int hash;
        public float InitialValue { get; set; }
        public BaseStatType StatType { get; }
        public virtual bool CheckCondition(float deltaTime)
        {
            return true;
        }


    }

}