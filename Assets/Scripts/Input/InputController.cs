using Math.Boards;
using Math.Enums;
using Math.Game;
using UnityEngine;

namespace Math.InputSytem
{
    public class InputController : MonoBehaviour
    {
        private GameController _gameController;
        private GridPosition _selectedGridPosition;
        private bool _isDragMode;

        public void Initialize(GameController gameController)
        {
            _gameController = gameController;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        public void UnsubscribeEvents()
        {
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        private void OnPointerDown(Vector2 pointerWorldPos)
        {
            _isDragMode = true;
            _gameController.SetSelectedItem(pointerWorldPos);
            //check move etrafını kontrol eden ve hareket edıp edemıcegı sonucunu donen bool fonk yaz 
        }

        private void OnPointerDrag(Vector2 pointerWorldPos)
        {
            if (!_isDragMode)
                return;

            if (!_gameController.CheckMove(pointerWorldPos))
            return;
            
            if (!_gameController.IsPointerOnBoard(pointerWorldPos, out GridPosition selectedGridPosition))
            {
                _isDragMode = false;
                return;
            }

            _selectedGridPosition = selectedGridPosition;
        }

        private void OnPointerUp(Vector2 pointerWorldPos)
        {
            _isDragMode = false;
        }

        // Her frame'de mobil dokunma girdilerini kontrol et
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0); // İlk dokunmayı al
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began: // Dokunma başladığında
                        OnPointerDown(touchPosition);
                        break;

                    case TouchPhase.Moved: // Dokunma hareket ederken
                        OnPointerDrag(touchPosition);
                        break;

                    case TouchPhase.Ended: // Dokunma sonlandığında
                    case TouchPhase.Canceled: // Dokunma iptal edildiğinde
                        OnPointerUp(touchPosition);
                        break;
                }
            }
        }
    }
}