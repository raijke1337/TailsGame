using UnityEngine;

namespace Arcatech.AI
{
    [CreateAssetMenu(fileName = "New EnemyStats", menuName = "Configurations/Enemy behavior Stats", order = 2)]
    public class EnemyStatsConfig : ScriptableObjectID
    {

        public float LookRange;
        public float LookSphereRadius;

        public float AttackRange;

        public ReferenceUnitType UnitType;

    }

}