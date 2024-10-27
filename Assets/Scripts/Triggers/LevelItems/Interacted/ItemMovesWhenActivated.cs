using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;

namespace Arcatech.Level
{
    public class ItemMovesWhenActivated : ConditionControlledItem
    {
        [SerializeField] SerializedDictionary<Rigidbody,Vector3> movingItemsToVector3;
        Vector3[] moveFrom;
        [SerializeField] bool loop = true;  

        [SerializeField] float movetime = 1f;
        [SerializeField] Ease ease = Ease.InOutQuad;

        bool activated = false; // activated ??

        private void OnValidate()
        {
            if (movingItemsToVector3 != null)
            {
                foreach (var rb in movingItemsToVector3.Keys)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
        }

        protected override void OnSetState(bool newstate)
        {

            if (activated && !newstate) return;
            {
                // case - disabled something
                // NYI

            }
            if (newstate)
            {
                int index = 0;
                moveFrom = new Vector3[movingItemsToVector3.Count];
                foreach (var item in movingItemsToVector3.Keys)
                {
                    moveFrom[index] = item.position;
                    if (loop)
                    {                        
                        item.DOMove(moveFrom[index] + movingItemsToVector3[item], movetime).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
                    }
                    else
                    {
                        item.DOMove(moveFrom[index] + movingItemsToVector3[item], movetime).SetEase(ease);
                    }
                }        
            }
        }
    }

}