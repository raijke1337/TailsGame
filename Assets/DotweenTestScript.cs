using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotweenTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var r = GetComponent<Rigidbody>();
     r.DOMove(transform.position + Vector3.forward*3,2f).SetLoops(-1,LoopType.Yoyo);   
    }
}
