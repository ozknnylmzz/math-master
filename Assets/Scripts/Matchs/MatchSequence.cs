using System.Collections.Generic;
using Math.Boards;
using Math.Enums;

namespace Matc3.Matchs
{
    public class MatchSequence
    {
        public IReadOnlyList<IGridSlot> MatchedGridSlots { get; }
        public MatchDetectorType MatchDetectorType { get; }

        public MatchSequence(IReadOnlyList<IGridSlot> matchedGridSlots,MatchDetectorType matchDetectorType)
        {
            MatchedGridSlots = matchedGridSlots;
            MatchDetectorType = matchDetectorType;
        }
    } 
}