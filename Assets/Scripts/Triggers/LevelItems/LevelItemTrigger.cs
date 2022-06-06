using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelItemTrigger : BaseTrigger,InteractiveItem
{
    [SerializeField] protected List<string> TriggerIDs;
    [SerializeField] protected bool disappearsOnPickup = true;
    [SerializeField] protected ParticleSystem pickupEffect;
    [SerializeField] protected float pickupEffectDuration = 0f;

    public InteractiveItemType IIType => InteractiveItemType.Pickup;

    protected override void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponent<PlayerUnit>();
        if (comp == null) return;
        foreach (var id in TriggerIDs)
        {
            TriggerCallback(id,comp,null);
        }
        if (pickupEffect!= null) StartCoroutine(ActivationEffect());
        if (disappearsOnPickup) StartCoroutine(BaseUnit.ItemDisappearsCoroutine(pickupEffectDuration,gameObject));
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

