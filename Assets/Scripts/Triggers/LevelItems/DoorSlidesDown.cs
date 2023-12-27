using System.Collections;
using UnityEngine;
namespace Arcatech.Triggers.Items
{
    public class ObjectMoves : ControlledItem
    {

        [SerializeField] protected float _desiredY = -2.5f;
        public override void DoControlAction(bool isP)
        {
            if (isP)
            StartCoroutine(MoveDown());
        }

        protected IEnumerator MoveDown()
        {

            Vector3 desired = new Vector3(transform.position.x, _desiredY, transform.position.z);

            while (transform.position.y > _desiredY)
            {
                transform.position += Vector3.Lerp(transform.position, desired, Time.deltaTime);
                yield return null;
            }
            yield return null;  
        }

    }
}