using System.Collections;
using UnityEngine;
namespace Arcatech.Triggers.Items
{
    public class ControlledItemMoves : ControlledItem
    {

        [SerializeField] protected Vector3 _desiredChangeLocalPosition;
        [SerializeField] protected float _movementTime;
        public override void DoControlAction(bool isP)
        {
            if (isP)
            StartCoroutine(MoveObject());
        }

        protected IEnumerator MoveObject()
        {
            float progress = 0;
            float koef = 1 / _movementTime;

            Vector3 desiredLocal = transform.localPosition + _desiredChangeLocalPosition;

            while (transform.localPosition != desiredLocal)
            {
                progress += Time.deltaTime * koef;
                transform.localPosition = Vector3.Lerp(transform.localPosition, desiredLocal, progress);
                yield return null;
            }
            yield return null;  
        }

    }
}