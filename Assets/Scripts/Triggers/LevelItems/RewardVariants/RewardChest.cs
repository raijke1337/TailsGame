using Arcatech.Triggers.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class RewardChest : LevelRewardTrigger
    {
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _itemSpot;
        [SerializeField] private ControlledItem _movingPart;

        protected override void OnEnter()
        {
            base.OnEnter();
            _movingPart.DoControlAction(true);
        }
        protected override void OnExit()
        {
            base.OnExit();
        }

    }
}