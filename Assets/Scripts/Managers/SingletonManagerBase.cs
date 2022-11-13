using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public abstract class SingletonManagerBase : MonoBehaviour
{
    
    public abstract void InitSingleton();
    public abstract void SetupManager();

}
