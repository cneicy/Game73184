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

        public TowerType TowerType;
        public int Cost;
        public GameObject TowerPrefab;
        public bool CanBuildOnRoad;
        public Sprite CardFace;
        public Sprite CardBack;
        public int ChargingIndex;
        
        private void Start()
        {
            isFlipped = false;
            _isSelected = false;
            _cardLocalPosition = transform.localPosition;
            _localTransform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateCardData(Card cardData)
        {
            TowerType = cardData.TowerType;
            Cost = cardData.Cost;
            TowerPrefab = cardData.TowerPrefab;
            CanBuildOnRoad = cardData.CanBuildOnRoad;
            CardFace = cardData.CardFace;
            CardBack = cardData.CardBack;
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
            BuildingSystem.Instance.NowCard = this;
            AfterBuilt();
            BuildingSystem.Instance.state = BuildingSystem.BuiltState.Building;
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
            
        }

        public void OnHoverExit()
        {
            
        }
    }
}
