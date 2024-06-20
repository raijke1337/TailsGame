using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Level
{
    public class MovingPlatformScript : MonoBehaviour
    {
        [SerializeField] Vector3 moveTo = Vector3.zero;
        [SerializeField] float movetime = 1f;

        [SerializeField] Ease ease = Ease.InOutQuad;

        Vector3 start;

        private void Start()
        {
            start = transform.position;
            DoMove();
        }
        void DoMove()
        {
            transform.DOMove(start + moveTo, movetime).
                SetEase(ease).
                SetLoops(-1, LoopType.Yoyo);
        }
    }
}