using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Puzzles
{
    public abstract class BasePuzzleComponent : MonoBehaviour
    {

        public SimpleEventsHandler<bool> PuzzleResult;
        protected void ResultCallback(bool isSolved)
        {
            PuzzleResult?.Invoke(isSolved);
        }
    }
}