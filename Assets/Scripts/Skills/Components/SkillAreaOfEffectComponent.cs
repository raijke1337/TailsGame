using Arcatech.Units;
using System.Collections;
using UnityEngine;

namespace Arcatech.Skills
{
    public class SkillAreaOfEffectComponent : MonoBehaviour
    {
        public event SimpleEventsHandler<BaseUnit> TargetHitEvent;
        public event SimpleEventsHandler SkillAreaDoneEvent;
        [HideInInspector] public SkillAoESettings Data;



        protected virtual void OnTriggerEnter(Collider other)
        {
            var comp = other.GetComponent<BaseUnit>();
            if (comp == null) return;
            TargetHitEvent?.Invoke(comp);
        }

        private void Awake()
        {
            var meshr = GetComponent<MeshRenderer>();
            meshr.enabled = false;

            Collider coll;
            if (!TryGetComponent(out coll))
            {
               coll = gameObject.AddComponent<Collider>();
            }
            coll.isTrigger = true;
        }
        private void Start()
        {
            StartCoroutine(ScalerCor());
        }

        private IEnumerator ScalerCor()
        {
            Vector3 originalScale = transform.localScale;
            float time = 0f;
            while (time < Data.GrowTime)
            {
                time += Time.deltaTime;
                float K = time / Data.GrowTime;
                transform.localScale = Vector3.Slerp(Data.StartRad, Data.EndRad, K);
                yield return null;
            }

            SkillAreaDoneEvent?.Invoke();
            Destroy(gameObject);
            yield return null;
        }
    }

}