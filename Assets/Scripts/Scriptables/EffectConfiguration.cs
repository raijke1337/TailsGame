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

public abstract class EffectConfiguration<T> : BaseEffectConfiguration where T: BaseCommandEffect
{
    [SerializeField]
    protected List<EffectData> _mainDatas;
    public abstract T CreateEffect(string ID);
}

public abstract class BaseEffectConfiguration : ScriptableObject
{

}

