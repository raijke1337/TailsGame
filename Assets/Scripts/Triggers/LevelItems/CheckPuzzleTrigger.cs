using Arcatech.Puzzles;

namespace Arcatech.Triggers
{
    public class CheckPuzzleTrigger : ControlItemTrigger
    {
        public BasePuzzleComponent PuzzlePrefab;

        public bool IsSolved { get; protected set; } = false;
        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
        public override void DoPositiveAction()
        {
            base.DoPositiveAction();
            IsSolved = true;
        }
        public override void DoNegativeAction()
        {
            base.DoNegativeAction();
            IsSolved = false;
        }
    }
}