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
        private Vector3 _limitedWorldPos;
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
            if (!_gameController.IsPointerOnBoard(pointerWorldPos, out GridPosition targetGridPosition))
            {
                Debug.Log("board üzerinde değil");
                _isDragMode = false;
                return;
            }
            _gameController.SetSelectedItem(targetGridPosition);
            _selectedGridPosition = targetGridPosition; // Başlangıç grid pozisyonunu kaydet
        }


        private void OnPointerDrag(Vector2 pointerWorldPos)
        {
            if (!_isDragMode)
                return;

            if (!_gameController.SelectedGridItem)
            {
                Debug.Log("_gameController.SelectedGridItem");
                return;
            }
            
            if (_gameController.IsPositionInLimited(pointerWorldPos, out Vector2 limitedPosition))
            {
                _gameController.ItemSetPosition(limitedPosition);
                _limitedWorldPos = limitedPosition;
            }
            
            if (!_gameController.IsPointerOnBoard(pointerWorldPos, out GridPosition targetGridPosition))
            {
                Debug.Log("board üzerinde değil");
                return;
            }
            
            Vector2 inputDirection = (Vector2)_gameController.SelectedGridItem.transform.localPosition- pointerWorldPos;

            if (_gameController.CheckMatch(inputDirection))
            {
                _gameController.SetItemToMove(pointerWorldPos);
                _isDragMode = false;
                return;
            }
          
            // Input yönünü hesaplayalım

            // Hareket kontrolü ve kilitleme işlemi
            if (!_gameController.CheckMove(pointerWorldPos, inputDirection))
            {
                _isDragMode = false; // Eğer hareket yapılamazsa drag modu devre dışı bırakılır
                return;
            }

            // Eğer yeni bir grid pozisyonuna geçtiysek, grid slot'larını güncelle
            if (!_selectedGridPosition.Equals(targetGridPosition))
            {
                _gameController.UpdateGridItemPosition(_selectedGridPosition, targetGridPosition);
                _selectedGridPosition = targetGridPosition;
            }
        }

        private void OnPointerUp(Vector2 pointerWorldPos)
        {
            if (!_gameController.SelectedGridItem)
            {
                Debug.Log("_gameController.SelectedGridItem");
                return;
            }
            // _gameController.SetItemToMove(pointerWorldPos);
            if (_gameController.JobCompleted)
            {
                return;
            }

            if (!_gameController.IsPointerOnBoard(pointerWorldPos, out GridPosition targetGridPosition))
            {
                Debug.Log("_limitedWorldPos ");
                _gameController.IsPointerOnBoard(_limitedWorldPos, out GridPosition _limitedWorldPosition);
               Debug.Log("_limitedWorldPosition"+_limitedWorldPosition);
                _gameController.SetItemToMove(_limitedWorldPos);
            }
            else
            {
                Debug.Log("SetItemToMove");
                _gameController.SetItemToMove(pointerWorldPos);
            }

          
            // _gameController.SetItemToMove(pointerWorldPos);
            _isDragMode = false;
        }
        
        private bool IsSideGrid(GridPosition gridPosition)
        {
            bool isSideGrid = gridPosition.Equals(_selectedGridPosition + GridPosition.Up) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Down) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Left) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Right);

            return isSideGrid;
        }

      
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