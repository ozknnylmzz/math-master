using System.Collections.Generic;
using Matc3.Matchs;
using Math.Boards;

namespace Math.Matchs
{
    public class MatchDataProvider :IMatchDataProvider
    {
        private readonly IMatchDetector[] _matchDetectors;

        public MatchDataProvider(IMatchDetector[] matchDetectors)
        {
            _matchDetectors = matchDetectors;
        }

        public BoardMatchData GetMatchData(IBoard board, params GridPosition[] gridPositions)
        {
            MatchedDataAllSlots matchedDataAllSlots = new MatchedDataAllSlots();

            foreach (GridPosition gridPosition in gridPositions)
            {
                UnionSharedData(matchedDataAllSlots, gridPosition, board);
            }

            return new BoardMatchData(matchedDataAllSlots.MatchDataList, matchedDataAllSlots.AllMatchedGridSlots);
        }

        private void UnionSharedData(MatchedDataAllSlots matchedDataAllSlots, GridPosition gridPosition,
            IBoard board)
        {
            HashSet<MatchSequence> matchSequences = GetMatchSequences(matchedDataAllSlots, gridPosition, board);
            if (matchSequences.Count > 0)
            {
                MatchData matchData = new MatchData(matchSequences);

                if (IsSharedMatchData(matchData, matchedDataAllSlots.MatchDataList,
                        out List<MatchData> sharedMatchDatas))
                {
                    foreach (MatchData sharedMatchData in sharedMatchDatas)
                    {
                        matchedDataAllSlots.MatchDataList.Remove(sharedMatchData);

                        matchSequences.UnionWith(sharedMatchData.MatchedSequences);

                        matchData.SetMatchDatas(matchSequences);
                    }
                }

                matchedDataAllSlots.MatchDataList.Add(matchData);
            }
        }

        private HashSet<MatchSequence> GetMatchSequences(MatchedDataAllSlots matchedDataAllSlots,
            GridPosition gridPosition, IBoard board)
        {
            HashSet<MatchSequence> matchSequences = new HashSet<MatchSequence>();

            foreach (IMatchDetector matchDetector in _matchDetectors)
            {
                MatchSequence sequence = matchDetector.GetMatchSequence(board, gridPosition);

                if (sequence == null)
                {
                    continue;
                }

                matchedDataAllSlots.AllMatchedGridSlots.UnionWith(sequence.MatchedGridSlots);

                matchSequences.Add(sequence);
            }

            return matchSequences;
        }

        private bool IsSharedMatchData(MatchData currentMatchData, List<MatchData> matchDataList,
            out List<MatchData> sharedMatchData)
        {
            bool isSharedFound = false;
            sharedMatchData = new List<MatchData>();
            foreach (MatchData matchData in matchDataList)
            {
                if (currentMatchData.MatchedGridSlots.Overlaps(matchData.MatchedGridSlots))
                {
                    sharedMatchData.Add(matchData);
                    isSharedFound = true;
                }
            }

            return isSharedFound;
        }
       
    }

    public class MatchedDataAllSlots
    {
        public List<MatchData> MatchDataList;
        public HashSet<IGridSlot> AllMatchedGridSlots;

        public MatchedDataAllSlots()
        {
            MatchDataList = new List<MatchData>();
            AllMatchedGridSlots = new HashSet<IGridSlot>();
        }
    }
}