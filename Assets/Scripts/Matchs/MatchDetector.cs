using System.Collections;
using System.Collections.Generic;
using Matc3.Matchs;
using Math.Boards;
using Math.Enums;
using Math.Matchs;
using UnityEngine;

namespace Math
{
    public class MatchDetector : IMatchDetector
    {

        public MatchSequence GetMatchSequence(IBoard board, GridPosition gridPosition)
        {
            throw new System.NotImplementedException();
        }

        public MatchDetectorType MatchDetectorType { get; }
    }
}
