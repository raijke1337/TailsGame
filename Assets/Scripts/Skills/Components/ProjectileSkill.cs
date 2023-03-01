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
    public void SetProjectileData(ProjectileDataConfig data) => ProjData = new ProjectileData(data);
    private ProjectileData ProjData;

    private float _exp;
    private int _penetr;
    public string GetID { get => SkillID;  }


    public void OnUse()
    {
        transform.position += transform.forward;
        _exp = ProjData.TimeToLive;
        _penetr = ProjData.Penetration;
    }

    public void OnUpdate(float delta)
    {
        if (transform == null) return; // case: gameobject was destroyed by manager
        transform.position += ProjData.Speed * delta * transform.forward;
        _exp -= delta;
        if (_exp <= 0f) CallExpiry();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other == Owner.GetCollider) return;
        base.OnTriggerEnter(other);
        if (_penetr > 0)
        {
            _penetr--;
        }
        if (_penetr == 0) CallExpiry();
    }


}

