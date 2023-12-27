using UnityEngine;

namespace Arcatech.AI
{
    [CreateAssetMenu(fileName = "New EnemyStats", menuName = "Configurations/EnemyStats", order = 2)]
    public class EnemyStatsConfig : ScriptableObjectID
    {

        public float LookRange;
        public float LookSphereRadius;

        public float AttackRange;

        public ReferenceUnitType UnitType;

    }

}