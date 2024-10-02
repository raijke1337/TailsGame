using DG.Tweening;
using UnityEngine;

namespace Arcatech.Level
{
    public class ActivatedPlatform : ConditionControlledItem
    {
        Rigidbody _rigidbody;

        Vector3 moveFrom;
        [SerializeField] Vector3 moveTo = Vector3.zero;

        [SerializeField] float movetime = 1f;

        [SerializeField] Ease ease = Ease.InOutQuad;


        protected override void OnSetState(bool newstate)
        {
            _rigidbody = GetComponent<Rigidbody>();
            moveFrom = transform.position;
            _rigidbody.DOMove(moveFrom + moveTo, movetime).SetLoops(-1, LoopType.Yoyo).SetEase(ease);

        }


    }

}