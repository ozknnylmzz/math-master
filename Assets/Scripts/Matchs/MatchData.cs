using System.Collections.Generic;
using Matc3.Matchs;
using Math.Boards;

namespace Math.Matchs
{
    public class MatchData
    {
        public HashSet<MatchSequence> MatchedSequences;
        public HashSet<IGridSlot> MatchedGridSlots;

        #region Variables

        private HashSet<MatchSequence> _matchedSequences
        {
            set => MatchedSequences = value;
        }

        private HashSet<IGridSlot> _matchedGridSlots
        {
            set => MatchedGridSlots = value;
        }

        #endregion

        public MatchData(HashSet<MatchSequence> matchedSequences)
        {
            SetMatchDatas(matchedSequences);
        }

        public void SetMatchDatas(HashSet<MatchSequence> matchedSequences)
        {
            _matchedSequences = matchedSequences;
            _matchedGridSlots = GetMatchedGridSlots();
        }

        private HashSet<IGridSlot> GetMatchedGridSlots()
        {
            HashSet<IGridSlot> matchedGridSlots = new();

            foreach (MatchSequence sequence in MatchedSequences)
            {
                matchedGridSlots.UnionWith(sequence.MatchedGridSlots);
            }

            return matchedGridSlots;
        }
      
    }
}