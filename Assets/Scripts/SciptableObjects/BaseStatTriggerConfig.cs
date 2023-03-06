using UnityEngine;

[CreateAssetMenu(fileName = "New BaseStatTriggerConfiguration", menuName = "Configurations/BaseStatTrigger", order = 1)]
public class BaseStatTriggerConfig : ScriptableObjectID
{
    public TriggerChangedValue ChangedValueType;
    public float InitialValue;
    public float RepeatedValue;
    public float RepeatApplicationDelay;
    public float TotalDuration;
    public Sprite Icon;

    public TriggerTargetType TargetType;
}

