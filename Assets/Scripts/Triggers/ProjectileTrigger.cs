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
using Zenject;

public class ProjectileTrigger : BaseTrigger, IProjectile
{
    public void SetProjectileData(ProjectileDataConfig data) => ProjData = new ProjectileData(data);

    private ProjectileData ProjData;

    private float _exp;
    private int _penetr;


    public event BaseUnitWithIDEvent TriggerHitEvent;
    public event SimpleEventsHandler<IProjectile> ExpiryEvent;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        if (other.CompareTag("StaticItem")) Stuck(other);
        if (other.GetComponent<BaseUnit>() != null)
        {
            Debug.Log($"{this} hit {other}");
            var tgt = other.GetComponent<BaseUnit>();
            foreach (var id in TriggerEffectIDs)
            {
                TriggerHitEvent?.Invoke(id,tgt);
            }
        }
        if (_penetr == 0) Stuck(other);
        _penetr--;
    }

    private void Stuck(Collider other)
    {
        ProjData.Speed = 0f;
        transform.parent = other.transform;
        _coll.enabled = false;
    }
    public void OnSpawn()
    {
        base.Awake();
        transform.position += transform.forward;
        _exp = ProjData.TimeToLive;
        _penetr = ProjData.Penetration;
    }

    public void OnUpdate()
    {
        transform.position += ProjData.Speed * Time.deltaTime * transform.forward;
        _exp -= Time.deltaTime;
        if (_exp <= 0) ExpiryEvent?.Invoke(this);
    }

    public void OnExpiry()
    {
        Destroy(gameObject);
    }

}

