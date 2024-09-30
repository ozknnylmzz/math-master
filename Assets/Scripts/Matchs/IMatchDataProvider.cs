using Math.Boards;

namespace Math.Matchs
{
    public interface IMatchDataProvider
    {
        public BoardDropItemData GetMatchData(IBoard board, GridPosition selectedPositions,GridPosition targetPositions);
    }
}