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

    public string ID;

    public string GetID { get => ID; }

    BaseUnit IProjectile.Source => Source;

    public event SimpleEventsHandler<IProjectile> ExpiryEventProjectile;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TextTrigger")) return;

        if (other.gameObject.isStatic) Stuck(other);

        base.OnTriggerEnter(other);

        if (ProjData.Penetration == 0) Stuck(other);
        ProjData.Penetration--;
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

        var oldx = transform.localEulerAngles.x;
        transform.Rotate(new Vector3(-oldx, 0, 0));
        transform.parent = null;
        // fix vertical rotation
    }

    public void OnUpdateProj()
    {
        transform.position += ProjData.Speed * Time.deltaTime * transform.forward;
        ProjData.TimeToLive -= Time.deltaTime;
        if (ProjData.TimeToLive <= 0) ExpiryEventProjectile?.Invoke(this);
    }

    public void OnExpiryProj()
    {
        Destroy(gameObject);
    }

}

