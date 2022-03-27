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

public class ProjectileTrigger : BaseTrigger
{
    private float _ttl;
    private float _speed;
    private float _pen;

    private bool isActive;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StaticItem")) Stuck(other);
        base.OnTriggerEnter(other);        
        if (_pen == 0) Stuck(other);
        _pen--;
    }

    private void Stuck(Collider other)
    {
        _speed = 0f;
        transform.parent = other.transform;
        _coll.enabled = false;
    }

    protected override void Start()
    {
        base.Start();
        _coll.enabled = false;
        StartCoroutine(ActivationTimer());

        Destroy(gameObject, _ttl);
    }

    private void Update()
    {
        transform.position += _speed * Time.deltaTime * transform.forward;
    }
    public void Setup(ProjectileConfig config,TriggersManager man)
    {
        _manager = man;
        _ttl = config.TimeToLive;
        _speed = config.ProjectileSpeed;
        _pen = config.ProjectilePenetration;
    }

    private IEnumerator ActivationTimer()
    {
        float time = 0f;
        while (time < Constants.Combat.c_ProjectileTriggerActivateDelay)
        {
            time += Time.deltaTime;
            yield return null;
        }
        _coll.enabled = true;
        yield return null;
    }
 
}

