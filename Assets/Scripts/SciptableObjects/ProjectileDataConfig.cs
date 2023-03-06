using UnityEngine;
[CreateAssetMenu(fileName = "New ProjectileConfig", menuName = "Configurations/Projectiles", order = 1)]
public class ProjectileDataConfig : ScriptableObjectID
{

    public float TimeToLive;
    public float ProjectileSpeed;
    [Range(1, 999)] public int ProjectilePenetration;

}

