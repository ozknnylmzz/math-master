using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Strategy;
using Math.Matchs;
using Math.Boards;
using Math.Enums;
using Math.Items;
using Math.Strategy;
using UnityEngine;

namespace Math.Game
{
    public class GameController : MonoBehaviour
    {
        private IBoard _board;
        private GridPosition _selectedGridPosition;
        private IMatchDataProvider _matchDataProvider;
        private MatchClearStrategy _matchClearStrategy;
        public GridItem SelectedGridItem { get; private set; }
        private GridItem _targetGridItem;
        public bool JobCompleted { get; private set; }


        public void Initialize(StrategyConfig strategyConfig, IBoard board, GameConfig gameConfig)
        {
            _board = board;
            _matchDataProvider = gameConfig.MatchDataProvider;
            _matchClearStrategy = strategyConfig.MatchClearStrategy;
        }
        
        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        public bool IsMatchDetected(GridPosition selectedPositions, GridPosition targetPositions)
        {
            if (selectedPositions==targetPositions)
            {
                return false;
            }
            if (_board.GetNormalItem(selectedPositions)?.ColorType != _board.GetNormalItem(targetPositions)?.ColorType)
            {
                return false;
            }
            
            CalculateMatch(SelectedGridItem,_targetGridItem);

            return true;
        }

        public void ItemSetPosition(Vector2 position)
        {
            SelectedGridItem.SetWorldPosition(position);
        }

        
        
        public bool CheckMove(Vector3 targetWorldPos, Vector2 inputDirection)
        {
            GridPosition currentGridPosition = SelectedGridItem.ItemSlot.GridPosition;
            bool canMoveX = true, canMoveY = true; // Başlangıçta hareket serbest

            // Sol, sağ, yukarı ve aşağıdaki slotları kontrol edelim
            GridPosition leftPosition = currentGridPosition + GridPosition.Left;
            GridPosition rightPosition = currentGridPosition + GridPosition.Right;
            GridPosition upPosition = currentGridPosition + GridPosition.Up;
            GridPosition downPosition = currentGridPosition + GridPosition.Down;

            // X yönünde hareket kontrolü
            if (inputDirection.x < 0) // Sola hareket
            {
                if (_board.IsPositionInBounds(leftPosition) && _board[leftPosition].HasItem)
                {
                    canMoveX = false; // Sol taraf dolu ise sola hareket kısıtlanır
                }
            }
            else if (inputDirection.x > 0) // Sağa hareket
            {
                if (_board.IsPositionInBounds(rightPosition) && _board[rightPosition].HasItem)
                {
                    canMoveX = false; // Sağ taraf dolu ise sağa hareket kısıtlanır
                }
            }

            // Y yönünde hareket kontrolü
            if (inputDirection.y > 0) // Yukarı hareket
            {
                if (_board.IsPositionInBounds(upPosition) && _board[upPosition].HasItem)
                {
                    canMoveY = false; // Yukarı taraf dolu ise yukarı hareket kısıtlanır
                }
            }
            else if (inputDirection.y < 0) // Aşağı hareket
            {
                if (_board.IsPositionInBounds(downPosition) && _board[downPosition].HasItem)
                {
                    canMoveY = false; // Aşağı taraf dolu ise aşağı hareket kısıtlanır
                }
            }

            // Hem X hem Y yönü kilitlenmişse, hareket edilemez
            if (!canMoveX && !canMoveY)
            {
                return false; // Eğer her iki yönde de hareket kısıtlanmışsa, hareket yapılmaz
            }

            // Eğer yalnızca X veya Y yönü serbestse o yönde hareket et
            Vector3 newPosition = SelectedGridItem.transform.position;

            if (canMoveX) // X ekseni boşsa
            {
                newPosition.x = targetWorldPos.x; // X ekseninde hareket et
            }

            if (canMoveY) // Y ekseni boşsa
            {
                newPosition.y = targetWorldPos.y; // Y ekseninde hareket et
            }

            ItemSetPosition(newPosition); // Yeni pozisyonu item'a setle
            return true;
        }


        public void SetSelectedItem(GridPosition gridPosition)
        {
            SelectedGridItem = _board.GetNormalItem(gridPosition);
        }

        public void SetItemToMove(Vector2 pointerWorldPos)
        {
            // Tahtadaki hedef grid slotu alıyoruz
            IGridSlot gridSlot = _board.GetGridSlot(pointerWorldPos);

            // Eğer bu slotun altında bir item yoksa, seçili item'in slotunu temizliyoruz
            if (!BoardHelper.IsItemBelow(gridSlot, _board, out IGridSlot targetSlot))
            {
                SelectedGridItem.ItemSlot.ClearSlot();
            }

            // if (BoardHelper.GetAllEmptySlotsBelow(gridSlot,_board,out ItemFallData fallData))
            // {
            //     
            // }

            // Grid slot'a seçili item'i yerleştiriyoruz
            gridSlot.SetItem(SelectedGridItem);
            SelectedGridItem.SetBoard(_board);
            SelectedGridItem.SetItemPosition(gridSlot.GridPosition);

            // Animasyon işlemleri için bir Sequence oluşturuyoruz
            Sequence sequence = DOTween.Sequence();

            // Eğer hedef slotun altında boş slotlar varsa, item düşüş animasyonu başlatıyoruz
            if (BoardHelper.GetAllEmptySlotsBelow(gridSlot, _board, out ItemFallData itemFallData))
            {
                // İlk slotu temizleyip, hedef slot'a item'i yerleştiriyoruz
                gridSlot.ClearSlot();
                SelectedGridItem.ItemSlot.ClearSlot();
                SelectedGridItem.SetItemPosition(gridSlot.GridPosition);
                targetSlot.SetItem(SelectedGridItem);

                // Animasyonla item'i hareket ettiriyoruz, ardından aynı fonksiyonu yeniden çağırıyoruz
                // SelectedGridItem?.DoMove(itemFallData.DestinationSlot).OnComplete(() => SetItemToMove(itemFallData.DestinationSlot.WorldPosition));

                // Eğer boş slotlar mevcutsa, item'lerin düşmesini sağlıyoruz
                if (itemFallData != null)
                {
                    Debug.Log("itemFallData: " + itemFallData.Item);
                    GetFallTween(itemFallData);
                }
            }
            // Eğer hedef slot boşsa, item'i buraya yerleştiriyoruz
            else if (!gridSlot.HasItem)
            {
                SelectedGridItem.ItemSlot.ClearSlot();
                gridSlot.SetItem(SelectedGridItem);
            }
        }

        private Tween GetFallTween(ItemFallData itemFallData)
        {
            // Grid slot'ları temizleyip yeni item'ları yerleştiriyoruz
            itemFallData.Item.ItemSlot.ClearSlot();
            itemFallData.DestinationSlot.SetItem(SelectedGridItem);

            // Düşecek olan item'in transform'una ve pathDistance'ına ulaş
            Transform item = itemFallData.Item.transform;
            float pathDistance = itemFallData.PathDistance;

            // Düşüş süresini hesapla
            float fallDuration = pathDistance <= 4
                ? 0.08f * pathDistance + 0.08f
                : pathDistance * 0.1f;

            // Sequence oluşturuyoruz
            Sequence sequence = DOTween.Sequence();

            // İlk item'in düşme animasyonunu ekliyoruz
            sequence.Append(item.DOMove(itemFallData.DestinationSlot.WorldPosition, fallDuration).SetEase(Ease.InSine));

            // Aynı anda diğer item'ların da düşmesini istiyorsan, diğer item'ları sırayla ekleyebilirsin
            BoardHelper.GetAllEmptySlotsBelow(itemFallData.DestinationSlot, _board, out ItemFallData nextItemFallData);

            while (nextItemFallData != null)
            {
                Transform nextItem = nextItemFallData.Item.transform;
                float nextItemPathDistance = nextItemFallData.PathDistance;

                float nextItemFallDuration = nextItemPathDistance <= 4
                    ? 0.08f * nextItemPathDistance + 0.08f
                    : nextItemPathDistance * 0.1f;

                // Diğer item'ların düşüşlerini ekle
                sequence.Join(nextItem.DOMove(nextItemFallData.DestinationSlot.WorldPosition, nextItemFallDuration)
                    .SetEase(Ease.InSine));

                BoardHelper.GetAllEmptySlotsBelow(nextItemFallData.DestinationSlot, _board, out nextItemFallData);
            }

            // Son olarak sequence tamamlandığında eşleşme kontrolü yapıyoruz
            sequence.OnComplete(() =>
            {
                BoardHelper.IsItemBelow(itemFallData.DestinationSlot, _board, out IGridSlot targetSlot);
                if (IsMatchDetected(SelectedGridItem.ItemSlot.GridPosition, targetSlot.GridPosition))
                {
                    Debug.Log("SelectedGridItem" + SelectedGridItem.ItemSlot.GridPosition);
                    _matchClearStrategy.CalculateMatchStrategyJobs(itemFallData.Item, targetSlot.Item);
                }
            });

            return sequence;
        }

        public void CalculateMatch(GridItem selectedGridItem,GridItem targetGridItem)
        {
            _matchClearStrategy.CalculateMatchStrategyJobs(selectedGridItem, targetGridItem);
        }

        public bool IsPositionInLimited(Vector2 position, out Vector2 limitedPosition)
        {
            limitedPosition = position;
            bool isOutOfBounds = false;

            // Y ekseninde sınır kontrolü
            if (position.y > _board.GetTopSlotY())
            {
                limitedPosition.y = _board.GetTopSlotY();
                isOutOfBounds = true;
            }
            else if (position.y < _board.GetBottomSlotY())
            {
                limitedPosition.y = _board.GetBottomSlotY();
                isOutOfBounds = true;
            }
            else
            {
                // Eğer Y ekseni sınır içindeyse, Y pozisyonunu orijinalde tut
                limitedPosition.y = position.y;
            }

            // X ekseninde sınır kontrolü
            if (position.x < _board.GetLeftSlotX())
            {
                limitedPosition.x = _board.GetLeftSlotX();
                isOutOfBounds = true;
            }
            else if (position.x > _board.GetRightSlotX())
            {
                limitedPosition.x = _board.GetRightSlotX();
                isOutOfBounds = true;
            }
            else
            {
                // Eğer X ekseni sınır içindeyse, X pozisyonunu orijinalde tut
                limitedPosition.x = position.x;
            }

            // Eğer pozisyon sınırların dışındaysa, limitedPosition güncellenmiş olacak
            return isOutOfBounds;
        }

        public Dictionary<IGridSlot, ItemFallData> CollectAllItemsToFall(IBoard board)
        {
            // Item'ların düşeceği verileri tutacağımız sözlük
            Dictionary<IGridSlot, ItemFallData> fallDataDictionary = new Dictionary<IGridSlot, ItemFallData>();

            // Tahtadaki her slotu kontrol ediyoruz
            foreach (var gridSlot in board.AllGridPositions)
            {
                // Eğer grid slot'ta bir item varsa ve altında boş slot varsa düşme verilerini topluyoruz
                IGridSlot currentSlot = board[gridSlot];
                if (currentSlot.Item != null &&
                    BoardHelper.GetAllEmptySlotsBelow(currentSlot, board, out ItemFallData fallData))
                {
                    // Düşme verilerini sözlüğe ekliyoruz (kaynak slot -> hedef slot)
                    fallDataDictionary[currentSlot] = fallData;
                }
            }

            return fallDataDictionary;
        }

        public void UpdateGridItemPosition(GridPosition previousPosition, GridPosition newPosition)
        {
            // Önceki ve yeni slot'ları al
            IGridSlot previousSlot = _board[previousPosition];
            IGridSlot newSlot = _board[newPosition];

            // Önceki slot'u temizle
            previousSlot.ClearSlot();

            // Yeni slot'a item'ı yerleştir
            newSlot.SetItem(SelectedGridItem);

            // Item'ın grid pozisyonunu güncelle
            // SelectedGridItem.SetItemPosition(newPosition);
        }

        public bool TryMoveItem(GridPosition targetPosition, Vector2 inputDirection)
        {
            GridPosition currentGridPosition = SelectedGridItem.ItemSlot.GridPosition;

            bool canMoveX = true, canMoveY = true; // Başlangıçta hareket serbest

            // Sol, sağ, yukarı ve aşağıdaki slotları kontrol edelim
            GridPosition leftPosition = currentGridPosition + GridPosition.Left;
            GridPosition rightPosition = currentGridPosition + GridPosition.Right;
            GridPosition upPosition = currentGridPosition + GridPosition.Up;
            GridPosition downPosition = currentGridPosition + GridPosition.Down;

            // X yönünde hareket kontrolü
            if (inputDirection.x < 0) // Sola hareket
            {
                if (_board.IsPositionInBounds(leftPosition) && _board[leftPosition].HasItem)
                {
                    canMoveX = false; // Sol taraf dolu ise sola hareket kısıtlanır
                }
            }
            else if (inputDirection.x > 0) // Sağa hareket
            {
                if (_board.IsPositionInBounds(rightPosition) && _board[rightPosition].HasItem)
                {
                    canMoveX = false; // Sağ taraf dolu ise sağa hareket kısıtlanır
                }
            }

            // Y yönünde hareket kontrolü
            if (inputDirection.y > 0) // Yukarı hareket
            {
                if (_board.IsPositionInBounds(upPosition) && _board[upPosition].HasItem)
                {
                    canMoveY = false; // Yukarı taraf dolu ise yukarı hareket kısıtlanır
                }
            }
            else if (inputDirection.y < 0) // Aşağı hareket
            {
                if (_board.IsPositionInBounds(downPosition) && _board[downPosition].HasItem)
                {
                    canMoveY = false; // Aşağı taraf dolu ise aşağı hareket kısıtlanır
                }
            }

            // Eğer hem X hem Y yönünde hareket kısıtlıysa, hareket yapılamaz
            if (!canMoveX && !canMoveY)
            {
                return false; // Eğer her iki yönde de hareket kısıtlanmışsa, hareket yapılmaz
            }

            // Eğer hareket yapılabiliyorsa, item'i yeni pozisyona setle ve önceki pozisyonu temizle
            if (canMoveX || canMoveY)
            {
                UpdateGridItemPosition(currentGridPosition, targetPosition); // Grid item'ı yeni pozisyona setle
                return true;
            }

            return false; // Eğer hareket yapılamazsa, false döndür
        }

        public bool CheckMatch(Vector2 inputDirection)
        {
            if (GetNextSlotByDirection(inputDirection).HasItem)
            {
                _targetGridItem=  GetNextSlotByDirection(inputDirection).Item;
            }
            
           if (_targetGridItem==SelectedGridItem)
           {
               return false;
           }

           if (_targetGridItem.ColorType==SelectedGridItem.ColorType)
           {
               CalculateMatch(SelectedGridItem,_targetGridItem);
               return true;
           }

           return false;
        }

        public IGridSlot GetNextSlotByDirection( Vector2 inputDirection)
        {
            // Yeni pozisyonu başta aynı olarak alıyoruz
            GridPosition nextGridPosition = SelectedGridItem.ItemSlot.GridPosition;

            // Yön X ekseninde ise (sağa veya sola hareket)
            if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            {
                if (inputDirection.x > 0) // Sağa doğru hareket
                {
                    nextGridPosition += GridPosition.Right;
                }
                else if (inputDirection.x < 0) // Sola doğru hareket
                {
                    nextGridPosition += GridPosition.Left;
                }
            }
            // Yön Y ekseninde ise (yukarı veya aşağı hareket)
            else
            {
                if (inputDirection.y > 0) // Yukarı doğru hareket
                {
                    nextGridPosition += GridPosition.Up;
                }
                else if (inputDirection.y < 0) // Aşağı doğru hareket
                {
                    nextGridPosition += GridPosition.Down;
                }
            }
            
            Debug.Log("nextgridpos"+nextGridPosition);

            // Yeni pozisyon tahtada geçerli mi kontrol edelim
            if (_board.IsPositionInBounds(nextGridPosition))
            {
                return _board[nextGridPosition]; // Yeni pozisyondaki slotu döndürüyoruz
            }

            return null; // Eğer pozisyon tahtada değilse null döner
        }

        
        public void ExecuteFallAnimations(Dictionary<IGridSlot, ItemFallData> fallDataDictionary)
        {
            // Sequence oluşturarak tüm düşüş animasyonlarını aynı anda başlatıyoruz
            Sequence sequence = DOTween.Sequence();

            foreach (var kvp in fallDataDictionary)
            {
                ItemFallData fallData = kvp.Value;

                // Item'ı hedef slot'a hareket ettir
                Transform itemTransform = fallData.Item.transform;
                float fallDuration = fallData.PathDistance <= 4
                    ? 0.08f * fallData.PathDistance + 0.08f
                    : fallData.PathDistance * 0.1f;

                // Tüm düşüşleri aynı anda başlatmak için Join kullanıyoruz
                sequence.Join(itemTransform.DOMove(fallData.DestinationSlot.WorldPosition, fallDuration)
                    .SetEase(Ease.InSine));
            }

            // Düşüş tamamlandığında işlem bitiyor
            sequence.OnComplete(() => { Debug.Log("Tüm item'lar düştü."); });

            // Sequence başlatılıyor
            sequence.Play();
        }
    }
}