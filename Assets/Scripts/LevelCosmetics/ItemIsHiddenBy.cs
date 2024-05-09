using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class ItemIsHiddenBy : MonoBehaviour
{
    [SerializeField] private MeshRenderer HiddenBy;
    private MeshRenderer _mr;

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        
    }
}
