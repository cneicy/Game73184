using GamePlay.Objects;
using GamePlay.Objects.Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.HandCard
{
    
    public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IInteractable,IHoverable
    {
        [SerializeField]private Vector3 _cardLocalPosition;
        private Transform _localTransform;
        private SpriteRenderer _spriteRenderer;
        private bool _isSelected;
        public bool isFlipped;
        [SerializeField] private GameObject shadowTowerObj;
        public CardData myCardData;

        private void Start()
        {
            isFlipped = false;
            _isSelected = false;
            _cardLocalPosition = transform.localPosition;
            _localTransform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateCardData(CardData cardData)
        {
            myCardData.towerType = cardData.towerType;
            myCardData.cost = cardData.cost;
            myCardData.towerPrefab = cardData.towerPrefab;
            myCardData.canBuildOnRoad = cardData.canBuildOnRoad;
            myCardData.cardFace = cardData.cardFace;
            myCardData.cardBack = cardData.cardBack;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            CardEffect.Instance.MouseEnter(_cardLocalPosition,_localTransform,_spriteRenderer);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CardEffect.Instance.MouseExit(_cardLocalPosition,_localTransform,_spriteRenderer);
        }

        public void AfterBuilt()
        {
            CardEffect.Instance.CardFlip(isFlipped,transform);
            isFlipped = !isFlipped;
        }
        
        private void BuildTower()
        {
            
        }
        
        public void Interact()
        {
            AfterBuilt();
            /*if (!_isSelected)
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
            }*/
        }

        public void OnHoverEnter()
        {
            print("鼠标进入卡牌");
        }

        public void OnHoverExit()
        {
            print("鼠标退出卡牌");
        }
    }
}
