using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "Configurations/EnemyStats", order = 2)]

public class EnemyStatsConfig : ScriptableObjectID
{

    public float lookRange;
    public float lookSphereRad;

    public float atkRange;

    public UnitType Type;

}

