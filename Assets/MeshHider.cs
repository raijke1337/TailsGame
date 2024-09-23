using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHider : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent<MeshRenderer>(out var m))
        {
            m.enabled= false;
        }
    }
}
