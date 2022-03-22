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

public class ProjectilesMover : MonoBehaviour
{
    private List<ProjectileComp> _items = new List<ProjectileComp>();
    // todo propper setup
    public void Add (ProjectileComp item)
    {
        _items.Add(item);
    }
    public void Remove(ProjectileComp item)
    {
        _items.Remove(item);
    }
    private void Update()
    {        
        foreach (var i in _items)
        {
            if (i.TTL <= 0f)
            {
                Destroy(i);
            }    
            else
            {
                i.transform.position += i.Speed * Time.deltaTime * i.transform.forward;
                i.TTL -= Time.deltaTime;
            }
        }
    }


}

