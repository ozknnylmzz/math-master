using Math.Game;
using Math.InputSytem;
using Math.Items;
using Math.Level;
using Math.Strategy;
using UnityEngine;

namespace Math.Boards
{
    public class BoardInitializer : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private ItemGenerator _itemGenerator;
        [SerializeField] private GameController _gameController;
        [SerializeField] private InputController _inputController;
        
        private GameConfig _gameConfig;
        private StrategyConfig _strategyConfig;
        
        public void Awake()
        {
            ConstructObjects();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // DOTween.Init().SetCapacity(500, 500);
            _gameConfig.Initialize();
            _levelGenerator.Initialize(_board,_itemGenerator);
            _board.Initialize();
            _strategyConfig.Initialize(_board, _itemGenerator);
            _gameController.Initialize(_strategyConfig,_board,_gameConfig);
            _inputController.Initialize(_gameController);
           
        }

        private void ConstructObjects()
        {
            _strategyConfig = new StrategyConfig();
            _gameConfig = new GameConfig();
            // _levelLoader = new LevelLoader();
        }
        
        private void OnDisable()
        {
             _inputController.UnsubscribeEvents();
        }

    }
}