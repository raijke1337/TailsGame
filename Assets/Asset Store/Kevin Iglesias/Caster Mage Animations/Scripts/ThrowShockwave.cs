﻿using UnityEngine;

namespace KevinIglesias
{

    public class ThrowShockwave : StateMachineBehaviour
    {

        CastSpells cS;

        public CastHand castHand;

        public float spawnDelay;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if (cS == null)
            {
                cS = animator.GetComponent<CastSpells>();
            }

            if (cS != null)
            {
                cS.ThrowShockwave(castHand, spawnDelay);
            }
        }
    }
}
