using Matc3.Matchs;
using Math.Boards;
using Math.Enums;

namespace Math.Matchs
{
    public interface IMatchDetector
    {
        public MatchSequence GetMatchSequence(IBoard board, GridPosition gridPosition);
        public MatchDetectorType MatchDetectorType { get; }
    } 
}