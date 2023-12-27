using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class RewardChest : LevelRewardTrigger
    {
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _itemSpot;
        [SerializeField] private Vector3 _desiredRotation = new Vector3(90f, 0, 0);

        protected override void OnEnter()
        {
            base.OnEnter();
            StartCoroutine(ChestOpen());
        }

        protected IEnumerator ChestOpen()
        {
            float progress = 0f;
            while (_top.eulerAngles.x <= _desiredRotation.x)
            {
                _top.eulerAngles = Vector3.Lerp(_top.eulerAngles, _desiredRotation, progress);
                progress += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
            yield return null;
        }
    }
}