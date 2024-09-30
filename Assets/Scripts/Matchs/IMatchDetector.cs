using Math.Boards;
using Math.Enums;

namespace Math.Matchs
{
    public interface IMatchDetector
    {
        public ItemFallData GetDropSequence(IBoard board, GridPosition gridPosition);
    } 
}