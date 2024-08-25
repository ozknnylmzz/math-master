using Math.Enums;
using Math.Boards;
using Math.Items;
using Math.Level;
using Unity.VisualScripting;
using UnityEngine;

namespace Math.Boards
{
    public class BoardInitializer : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private ItemGenerator _itemGenerator; 
        public void Awake()
        {
            ConstructObjects();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // DOTween.Init().SetCapacity(500, 500);
            _levelGenerator.Initialize(_board,_itemGenerator);
            _board.Initialize();
        }

        private void ConstructObjects()
        {
            // _strategyConfig = new StrategyConfig();
            // _gameConfig = new GameConfig();
            // _levelLoader = new LevelLoader();
        }
        
        private void OnDisable()
        {
            // _inputController.UnsubscribeEvents();
        }

        private void OnDestroy()
        {
            // EventBus.Instance.Publish(BoardEvents.OnBoardDestroyed);
        }
    }
}