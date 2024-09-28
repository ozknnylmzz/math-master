using Math.Boards;

namespace Math.Matchs
{
    public interface IMatchDataProvider
    {
        public BoardMatchData GetMatchData(IBoard board, GridPosition[] gridPositions);
    }
}