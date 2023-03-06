using UnityEngine;
[RequireComponent(typeof(Collider))]
public abstract class BaseSkill : MonoBehaviour, IAppliesTriggers, IHasOwner, IExpires
{
    public string SkillID;
    public BaseUnit Owner { get; set; }
    [HideInInspector] public SkillData SkillData;
    public SkillAreaComp EffectPrefab;

    public event TriggerEventApplication TriggerApplicationRequestEvent;

    public event SimpleEventsHandler<IExpires> HasExpiredEvent;

    private Collider _coll;
    // creates new objects when conditions are met
    // ie explosive bolt explodes into shrapnel

    private SkillAreaComp _placedEffect;


    protected virtual void OnTriggerEnter(Collider other)
    {
        _placedEffect = PlaceAndSubEffect(transform.position);
        _coll.enabled = false;
        _placedEffect.SkillAreaDoneEvent += CallExpiry; // todo?
    }


    private void Awake()
    {
        if (EffectPrefab == null || EffectPrefab.GetComponent<SkillAreaComp>() == null) Debug.LogError($"Set prefab for {this}");
        _coll = GetComponent<Collider>();
        _coll.isTrigger = true;
    }

    // use this to apply all trigger effects set in SKillData
    protected virtual void CallTriggerHit(BaseUnit target)
    {
        if (SkillData.TriggerIDs == null) return;
        foreach (var id in SkillData.TriggerIDs)
        {
            TriggerApplicationRequestEvent?.Invoke(id, target, Owner);
        }
    }
    protected virtual void CallExpiry() { HasExpiredEvent?.Invoke(this); }


    protected SkillAreaComp PlaceAndSubEffect(Vector3 tr)
    {
        var item = Instantiate(EffectPrefab).GetComponent<SkillAreaComp>();
        item.Data = SkillData;
        item.transform.parent = null;
        item.transform.position = tr;
        item.TargetHitEvent += (t) => CallTriggerHit(t);
        return item;
    }

    public GameObject GetObject() => gameObject;



}





