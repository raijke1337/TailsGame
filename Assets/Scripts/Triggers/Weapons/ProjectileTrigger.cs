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
    public event SimpleEventsHandler<IExpires> HasExpiredEvent;

    public string GetID { get => ID; }

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
    public void OnUse()
    {
        base.Awake();

        var oldx = transform.localEulerAngles.x;
        transform.Rotate(new Vector3(-oldx, 0, 0));
        transform.parent = null;
        // fix vertical rotation
    }

    public void OnUpdate(float delta)
    {
        transform.position += ProjData.Speed * delta * transform.forward;
        ProjData.TimeToLive -= delta;
        if (ProjData.TimeToLive <= 0) HasExpiredEvent?.Invoke(this);
    }
}

