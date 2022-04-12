using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class StunnerComponent : IStatsComponentForHandler
    {
        public int StaggerCounter { get; } // total hits it takes to stagger a unit
        private int currentCount; // current hits left to stagger
        private float counterReset; // how long it takes to reset stun counter
        private float currentReset; // current time

        public StunnerComponent(int count, int countreset)
        {
            StaggerCounter = currentCount = count;
            counterReset = countreset;
            currentReset = 0;
        }

        public bool DidHitStun(bool advanceCounter = true)
        {
            currentReset = 0f;
            if (advanceCounter)
            {
                currentCount++;
            }
            if (currentCount == StaggerCounter)
            {
                return true;
            }
            else return false;
        }

        public void UpdateInDelta(float deltaTime)
        {
            if (currentReset < counterReset) { currentReset += deltaTime; }
            else if (currentReset >= counterReset && currentCount != 0)
            {
                currentCount = 0;
            }
        }

        public void SetupStatsComponent()
        {

        }
    }
}