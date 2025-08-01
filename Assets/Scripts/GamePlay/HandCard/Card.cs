using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GamePlay.HandCard
{
    public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IInteractable
    {
        [SerializeField]private Vector3 _cardLocalPosition;
        private Transform _localTransform;
        private SpriteRenderer _spriteRenderer;
        private bool _isSelected;
        [SerializeField] private GameObject shadowTowerObj;

        private void Start()
        {
            _isSelected = false;
            _cardLocalPosition = transform.localPosition;
            _localTransform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CardEffect.Instance.MouseEnter(_cardLocalPosition,_localTransform,_spriteRenderer);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CardEffect.Instance.MouseExit(_cardLocalPosition,_localTransform,_spriteRenderer);
        }

        private void BuildTower()
        {
            
        }
        
        public void Interact()
        {
            if (!_isSelected)
            {
                _cardLocalPosition = transform.localPosition;
                CardEffect.Instance.MouseEnter(_cardLocalPosition,_localTransform,_spriteRenderer);
                _isSelected = true;
            }
            else
            {
                _cardLocalPosition = transform.localPosition;
                CardEffect.Instance.MouseExit(_cardLocalPosition,_localTransform,_spriteRenderer);
                _isSelected = false;
            }
        }
    }
}
