using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsChangingTrigger : BaseTrigger
{
    [SerializeField] protected List<string> TriggerIDs;
    [SerializeField] protected bool disappearsOnPickup = true;
    [SerializeField] protected ParticleSystem pickupEffect;
    [SerializeField] protected float pickupEffectDuration = 0f;


    protected override void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponent<PlayerUnit>();
        if (comp == null) return;
        foreach (var id in TriggerIDs)
        {
            TriggerCallback(id, comp, null);
        }
        if (pickupEffect != null) StartCoroutine(ActivationEffect());
        if (disappearsOnPickup) Destroy(gameObject);
    }

    protected IEnumerator ActivationEffect()
    {
        pickupEffect.Play();
        float passed = 0f;
        while (passed < pickupEffectDuration)
        {
            passed += Time.deltaTime;
            yield return null;
        }
        pickupEffect.Stop();
        yield return null;
    }
}

