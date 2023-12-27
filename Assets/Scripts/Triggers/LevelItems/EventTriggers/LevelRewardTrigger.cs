using Arcatech.Items;
using CartoonFX;

namespace Arcatech.Triggers
{
    public class LevelRewardTrigger : BaseLevelEventTrigger
    {
        public Item Content;
        public CFXR_Effect Effect;

        protected override void OnEnter()
        {
            if (Effect != null)
            {
                Instantiate(Effect, transform);
            }
        }

        protected override void OnExit()
        {
        }
    }
}