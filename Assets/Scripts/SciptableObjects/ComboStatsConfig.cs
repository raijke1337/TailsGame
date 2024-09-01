
using Arcatech.Stats;
using UnityEngine;
namespace Arcatech
{

    [CreateAssetMenu(fileName = "New HeatStatsConfig", menuName = "Configurations/Combo")]
    public class ComboStatsConfig : ScriptableObjectID
    {
        [SerializeField] public StatValueContainer ComboContainer;
        public float DegenCoeff;
        public float HeatTimeout;
    }

}