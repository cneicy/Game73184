using Singleton;
using UnityEngine.EventSystems;

namespace GamePlay.HandCard
{
    public class CardDrag : Singleton<CardDrag>,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
