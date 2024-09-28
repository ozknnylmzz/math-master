using System.Collections.Generic;
using Math.Matchs;

namespace Math.Boards
{
    public class BoardMatchData
    {
        private IReadOnlyList<MatchData> _matchedDataList { get; }

        public IReadOnlyCollection<IGridSlot> AllMatchedGridSlots { get; }

        public bool MatchExists => _matchedDataList.Count != 0;

        public BoardMatchData(IReadOnlyList<MatchData> matchedDataList, IReadOnlyCollection<IGridSlot> allMatchedGridSlots)
        {
            _matchedDataList = matchedDataList;
            AllMatchedGridSlots = allMatchedGridSlots;
        }
    } 
}