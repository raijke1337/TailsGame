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

public class ProjectileSkill : BaseSkill, IProjectile
{
    public event BaseUnitWithIDEvent TriggerHitEvent;
    public event SimpleEventsHandler<IProjectile> ExpiryEvent;

    public void SetProjectileData(ProjectileDataConfig data) => ProjData = new ProjectileData(data);
    private ProjectileData ProjData;

    private float _exp;
    private int _penetr;

    public void OnExpiry()
    {
        Destroy(gameObject);
    }

    public void OnSpawn()
    {
        transform.position += transform.forward;
        _exp = ProjData.TimeToLive;
        _penetr = ProjData.Penetration;
    }

    public void OnUpdate()
    {
        transform.position += ProjData.Speed * Time.deltaTime * transform.forward;
        _exp -= Time.deltaTime;
        if (_exp <= 0f) ExpiryEvent?.Invoke(this);
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ground")) return;
        Debug.Log(other);
        if (_penetr > 0)
        {
            var expl = Instantiate(EffectPrefab).GetComponent<SkillAreaComp>();
            expl.transform.position = other.transform.position;
            expl.TargetHitEvent += (t) => TriggerHitEvent?.Invoke(SkillID, t);
            expl.Data = SkillData;
            _penetr--;
        }
        if (_penetr == 0) ExpiryEvent?.Invoke(this);
    }



}

