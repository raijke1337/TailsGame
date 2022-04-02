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
using RotaryHeart.Lib.SerializableDictionary;

public class CrosshairScript : MonoBehaviour
{
    [SerializeField] private float _aimOffset = 0.1f;

    private Vector3 _adjustedTgt;
    private SpriteRenderer _pic;



    [SerializeField] private CursorType CrosshairState;

    [SerializeField] SerializableDictionaryBase<CursorType, Sprite> Shapes;

    public void SetLookTarget(Vector3 target)
    {
        _adjustedTgt = new Vector3(target.x, _aimOffset, target.z);
        transform.position = _adjustedTgt;
    }

    private void Start()
    {
        _pic = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        _pic.sprite = Shapes[CrosshairState];
        transform.position = _adjustedTgt;
    }

}

