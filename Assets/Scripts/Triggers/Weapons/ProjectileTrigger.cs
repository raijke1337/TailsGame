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

public class ProjectileTrigger : WeaponTriggerComponent, IProjectile
{
    public void SetProjectileData(ProjectileDataConfig data) => ProjData = new ProjectileData(data);
    private ProjectileData ProjData;

    private float _exp;
    private int _penetr;

    public string GetID { get; set; }

    BaseUnit IProjectile.Source => Source;

    public event SimpleEventsHandler<IProjectile> ExpiryEventProjectile;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TextTrigger")) return;

        if (other.gameObject.isStatic) Stuck(other);

        base.OnTriggerEnter(other);

        if (_penetr == 0) Stuck(other);
        _penetr--;
    }

    private void Stuck(Collider other)
    {
        ProjData.Speed = 0f;
        transform.parent = other.transform;
        _coll.enabled = false;
    }
    public void OnSpawnProj()
    {
        base.Awake();
        _exp = ProjData.TimeToLive;
        _penetr = ProjData.Penetration;

        var oldx = transform.localEulerAngles.x;
        transform.Rotate(new Vector3(-oldx, 0, 0));
        // fix vertical rotation
    }

    public void OnUpdateProj()
    {
        transform.position += ProjData.Speed * Time.deltaTime * transform.forward;
        _exp -= Time.deltaTime;
        if (_exp <= 0) ExpiryEventProjectile?.Invoke(this);
    }

    public void OnExpiryProj()
    {
        Destroy(gameObject);
    }

}

