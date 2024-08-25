using TMPro;
using UnityEngine;

namespace Math.Items
{
    public abstract class SpriteItem : GridItem
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected TextMeshProUGUI _text;

        private int _initialSortingOrder;

        protected void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        protected void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        protected void SetText(string textValue)
        {
            _text.text = textValue;
        }
        
        protected void SetTextColor(Color color)
        {
            _text.color = color;
        }

        public override void Initialize()
        {
            base.Initialize();
            _initialSortingOrder = _spriteRenderer.sortingOrder;
        }

        public override void ResetItem()
        {
            base.ResetItem();
            ResetLayer();
        }

        private void ResetLayer()
        {
            _spriteRenderer.sortingOrder = _initialSortingOrder;
        }
    }
}