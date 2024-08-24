using Math.Enums;
using Math.Boards;
using Unity.VisualScripting;
using UnityEngine;

namespace Math.Boards
{
    public class BoardInitializer : MonoBehaviour
    {
        [SerializeField] private Board _board;

        public void Awake()
        {
            ConstructObjects();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // DOTween.Init().SetCapacity(500, 500);
           
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