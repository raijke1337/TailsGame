using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class test : MonoBehaviour
{
    public Vector3 startingR;
    public Vector3 endingR;
    public float time = 1f;
    public bool doRotation = false;

    private bool busy = false;

    private void OnEnable()
    {
        startingR = transform.localEulerAngles;

    }

    private void Update()
    {
        if (doRotation)
        {
            if (busy) doRotation = false;
            else
            {
                busy = true;
                StartCoroutine(Rotator());
            }
        }
    }

    private IEnumerator Rotator()
    {
        float progress = 0;
        while (progress < 1)
        {
            progress += 1 / time * Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startingR, endingR, progress);
            yield return null;
        }
        busy = false;
        yield return null; 
        
    }
}
