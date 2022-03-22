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

[RequireComponent(typeof(BaseWeaponController),typeof(BaseUnit))]
public abstract class BaseUnitController : MonoBehaviour
{
    protected BaseWeaponController _baseWeap;
    public BaseUnit GetUnit { get; protected set; }

    protected virtual void Awake()
    {
        _baseWeap = GetComponent<BaseWeaponController>();
        GetUnit = GetComponent<BaseUnit>();
    }

    public ref Vector3 MoveDirection => ref _movement;
    protected Vector3 _movement;


}

