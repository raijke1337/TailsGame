using Arcatech.Items;
using Arcatech.Triggers.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class RewardChest : LevelRewardTrigger
    {
//        [SerializeField] private Transform _top;
        [SerializeField] private ControlledItem _movingPart;



        protected override void OnEnter()
        {
            base.OnEnter();
            _movingPart.ChangeItemState(ControlledItemState.Positive);
        }
        protected override void OnExit()
        {
            _movingPart.ChangeItemState(ControlledItemState.Negative);
            _movingPart.ItemChangedStateEvent += DisappearWhenClosed;
        }

        private void DisappearWhenClosed(ControlledItemState arg1, ControlledItem arg2)
        {
            if (arg1 == ControlledItemState.Negative) // should not trigger on lvl start because there is no triggerexit
            {
                _movingPart.ItemChangedStateEvent -= DisappearWhenClosed;
                ItemIsGone();
            }
        }
    }
}