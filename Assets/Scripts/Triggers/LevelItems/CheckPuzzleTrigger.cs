using Arcatech.Managers;
using Arcatech.Puzzles;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Triggers
{
    public class CheckPuzzleTrigger : CheckConditionTrigger
    {
        [SerializeField] protected BasePuzzleComponent PuzzlePrefab; 
        protected BasePuzzleComponent _currentPuzzleWindow;
        protected GameInterfaceManager _ui;

        public bool IsSolved { get; protected set; } = false;


        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_ui == null)
                {
                    _ui = GameManager.Instance.GetGameControllers.GameInterfaceManager;
                }
                if (!IsSolved)
                {
                    _currentPuzzleWindow = Instantiate(PuzzlePrefab, _ui.transform);
                    _currentPuzzleWindow.PuzzleResult += OnPuzzleResult;

                    _ui.OnPauseRequesShowPanelAndPause(true);
                }

                base.OnTriggerEnter(other);
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            _currentPuzzleWindow = null;
            base.OnTriggerExit(other);
        }
        protected override bool CheckTheCondition()
        {
            return IsSolved;
        }



        private void OnPuzzleResult(bool ok)
        {
            _currentPuzzleWindow.PuzzleResult -= OnPuzzleResult;
            _ui.OnPauseRequesShowPanelAndPause(false);
            Destroy(_currentPuzzleWindow.gameObject);
            IsSolved = ok;
        }


    }
}