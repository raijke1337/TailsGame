using Arcatech.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
namespace Arcatech.Skills
{
    [RequireComponent(typeof(SphereCollider),typeof(ProjectileComponent))]
    public class SphereScaleScript : MonoBehaviour
    {
        SphereCollider _col;
        ProjectileComponent _projectile;
        [SerializeField] float DesiredRadius;
        Coroutine scaler;
        float time;

        private void Start()
        {
            _projectile = GetComponent<ProjectileComponent>();
            _col = GetComponent<SphereCollider>();
            time = _projectile.Lifetime;

            scaler = StartCoroutine(Scale());
        }
        protected IEnumerator Scale()
        {
            _col.radius = 0.1f;
            float coef = DesiredRadius / time;
            while (_col.radius <= DesiredRadius)
            {
                _col.radius += Time.deltaTime * coef;
                yield return null;
            }
            yield return null;
        }
        private void OnDestroy()
        {
            StopCoroutine(scaler);
        }

        private void OnValidate()
        {
            Assert.IsFalse(DesiredRadius == 0);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DesiredRadius);
        }
    }
}