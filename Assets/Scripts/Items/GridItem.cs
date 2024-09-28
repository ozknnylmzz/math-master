using Math.Enums;
using Math.Boards;
using UnityEngine;

namespace Math.Items
{
    public abstract class GridItem : MonoBehaviour
    {
        public abstract ItemType ItemType { get; }
        public int ConfigureType { get; private set; } = 0;

        public abstract void ConfigureItem(int configureType);

        public ColorType ColorType { get; private set; } = ColorType.None;

        public bool IsMatchable => ColorType != ColorType.None;

        public IGridSlot ItemSlot { get; private set; }

        public IGridSlot DestinationSlot { get; private set; }

        public int ItemStateDelay { get; private set; } = 0;

        public int PathDistance { get; private set; } = 0;

        public IBoard Board  { get; private set; }

        private ItemGenerator _generator;

        public virtual void Initialize() {}

        private void SetScale(float scale)
        {
            transform.SetScale(scale);
        }

        public void SetSlot(IGridSlot slot)
        {
            ItemSlot = slot;
        }

        public void SetGenerator(ItemGenerator generator)
        {
            _generator = generator;
        }

        public void SetBoard(IBoard board)
        {
            Board = board;
        }
        
        public void SetItemPosition(GridPosition gridPosition)
        {
          transform.position =  Board.GridToWorldPosition(gridPosition);
        }

     

        public void ReturnToPool()
        {
            _generator.ReturnItemToPool(this);
        }

        public virtual void ResetItem()
        {
            SetScale(1);
            SetItemStateDelay(0);
        }

        protected void SetConfigureType(int configureType)
        {
            ConfigureType = configureType;
        }

        protected void SetColorType(ColorType colorType)
        {
            ColorType = colorType;
        }

        public void SetDestinationSlot(IGridSlot destinationSlot)
        {
            DestinationSlot = destinationSlot;
        }

        /// <summary>
        ///   <para>shouldPlayExplosion: explosion particle oynasin mi</para>
        ///   <para>isSpecialKill: 4lu tas mi 8li tas mi</para>
        /// </summary>
        public virtual void Kill(bool shouldPlayExplosion = true, bool isSpecialKill = false)
        {
            this.Hide();
        }
        
        private void SetItemStateDelay(int value)
        {
            ItemStateDelay = value;
        }

        public void SetPathDistance(int pathDistance)
        {
            PathDistance = pathDistance;
        }

        public void ResetPathDistance()
        {
            PathDistance = 0;
        }
    }
}