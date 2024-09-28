using Match3.Strategy;
using Math.Boards;
using Math.Items;

namespace Math.Strategy
{
    public class StrategyConfig
    {
        private FillStrategy FillStrategy { get; set; }
        private BoardClearStrategy BoardClearStrategy { get; set; }
        public MatchClearStrategy MatchClearStrategy { get; private set; }

        public void Initialize(IBoard board,ItemGenerator itemGenerator)
        {
            FillStrategy = new FillStrategy(board, itemGenerator);
            BoardClearStrategy = new BoardClearStrategy(FillStrategy);
            MatchClearStrategy = new MatchClearStrategy(BoardClearStrategy);
        }
    }
}