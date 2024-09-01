using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Skills
{
    [RequireComponent(typeof(SphereCollider))]
    public class SphereScaleScript : MonoBehaviour
    {
        SphereCollider _col;
        float rad;
        Coroutine scaler;
        private void Awake()
        {
            _col = GetComponent<SphereCollider>();
            rad = _col.radius;

            scaler = StartCoroutine(Scale());
        }
        protected IEnumerator Scale()
        {
            _col.radius = 0f;
            while (_col.radius <= rad)
            {
                _col.radius += Time.deltaTime * 10;
                yield return null;
            }
            yield return null;
        }
        private void OnDestroy()
        {
            StopCoroutine(scaler);
        }

    }
}