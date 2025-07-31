using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GamePlay.HandCard
{
    public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler
    {
        private Vector3 _cardLocalPosition;
        private Transform _localTransform;

        private void Start()
        {
            _cardLocalPosition = transform.localPosition;
            _localTransform = transform;
            print(_cardLocalPosition);
            print(transform.position);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            print("111");
            CardHover.Instance.MouseEnter(_cardLocalPosition,_localTransform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            print("222");
            CardHover.Instance.MouseExit(_cardLocalPosition,_localTransform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}
