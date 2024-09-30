using System.Collections;
using System.Collections.Generic;
using Math.Boards;
using Math.Enums;
using Math.Matchs;
using UnityEngine;

namespace Math.Matchs
{
    public class MatchDetector : IMatchDetector
    {
        public ItemFallData GetDropSequence(IBoard board, GridPosition gridPosition)
        {
            throw new System.NotImplementedException();
        }

        public MatchDetectorType MatchDetectorType { get; }
    }
}
