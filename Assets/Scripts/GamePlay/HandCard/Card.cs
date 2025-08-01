using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.HandCard
{
    public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IInteractable
    {
        [SerializeField]private Vector3 _cardLocalPosition;
        private Transform _localTransform;
        private SpriteRenderer _spriteRenderer;
        private bool _isSelected;

        private void Start()
        {
            _isSelected = false;
            _cardLocalPosition = transform.localPosition;
            _localTransform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CardHover.Instance.MouseEnter(_cardLocalPosition,_localTransform,_spriteRenderer);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CardHover.Instance.MouseExit(_cardLocalPosition,_localTransform,_spriteRenderer);
        }

        public void Interact()
        {
            if (!_isSelected)
            {
                _cardLocalPosition = transform.localPosition;
                CardHover.Instance.MouseEnter(_cardLocalPosition,_localTransform,_spriteRenderer);
                _isSelected = true;
            }
            else
            {
                _cardLocalPosition = transform.localPosition;
                CardHover.Instance.MouseExit(_cardLocalPosition,_localTransform,_spriteRenderer);
                _isSelected = false;
            }
        }
    }
}
