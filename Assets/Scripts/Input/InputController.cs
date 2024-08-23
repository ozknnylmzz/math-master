using UnityEngine;
using UnityEngine.EventSystems;

namespace Math.Input
{
    public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Vector3 offset;
        private bool isDragging;

        // Pointer Down Event: Obje tıklanıp sürüklemeye başlandığında
        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            // Obje ile fare arasındaki mesafeyi hesapla
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
            mousePosition.z = 0; // Z pozisyonunu sıfırlamak
            offset = transform.position - mousePosition;
        }

      

        // Pointer Up Event: Obje bırakıldığında çağrılır
        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            // Burada objeyi grid sistemine hizalama kodunu ekleyebilirsin
            SnapToGrid();
        }

        // Objeyi en yakın grid pozisyonuna hizala
        private void SnapToGrid()
        {
            float gridSize = 1.0f; // Grid boyutu
            float newX = Mathf.Round(transform.position.x / gridSize) * gridSize;
            float newY = Mathf.Round(transform.position.y / gridSize) * gridSize;
            transform.position = new Vector3(newX, newY, 0); // Z pozisyonunu sıfırla
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                // Yeni pozisyonu hesapla
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
                mousePosition.z = 0; // Z pozisyonunu sıfırlamak
                transform.position = mousePosition + offset; // Objenin pozisyonunu güncelle
            }
        }
    }
}
