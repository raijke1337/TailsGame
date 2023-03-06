using UnityEngine;
[CreateAssetMenu(fileName = "New Stuns COntroller Config", menuName = "Configurations/Stuns")]
public class StunsControllerConfig : ScriptableObjectID
{
    public StatValueContainer StunResistance;
    public float RegenPerSec;
    public float GracePeriod;
}

