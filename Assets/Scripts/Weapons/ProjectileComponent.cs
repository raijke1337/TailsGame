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

public class ProjectileComponent : MonoBehaviour
{    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit tgt");        
    }

    private void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime;  
    }

}

