using System;
using GamePlay.Objects;
using GamePlay.Objects.Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GamePlay.HandCard
{
    
    
    
    public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IInteractable,IHoverable
    {
        [SerializeField]private Vector3 _cardLocalPosition;
        private Transform _localTransform;
        private SpriteRenderer _spriteRenderer;
        private bool _isSelected;
        public bool isFlipped;

        public TowerType TowerType;
        public int Cost;
        public GameObject TowerPrefab;
        public bool CanBuildOnRoad;
        public Sprite CardFace;
        public Sprite CardBack;
        //充能
        public float maxChargingIndex;
        public float nowChargingIndex;
        private float _maskProportion;
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
            maxChargingIndex = cardData.maxChargingIndex;
            LoadCardData();
        }

        private void LoadCardData()
        {
            //花费
            //充能
            nowChargingIndex = maxChargingIndex;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CardEffect.Instance.MouseEnter(_cardLocalPosition,_localTransform,_spriteRenderer);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CardEffect.Instance.MouseExit(_cardLocalPosition,_localTransform,_spriteRenderer);
        }


        public void Waitting()
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

        private void Update()
        {
            if (BuildingSystem.Instance.state==BuildingSystem.BuiltState.Building)
            {
                nowChargingIndex = BuildingSystem.Instance.NowCard.nowChargingIndex;
                _maskProportion = nowChargingIndex / maxChargingIndex;
            }
        }


        public void ChangeMask()
        {
            
        }
        
        public void Interact()
        {
            if (BuildingSystem.Instance.state==BuildingSystem.BuiltState.Waiting)
            {
                BuildingSystem.Instance.NowCard = this;
                BuildingSystem.Instance.ChangeStateToBuilding();
            }
            
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

        public void OnHoverEnter()
        {
            
        }

        public void OnHoverExit()
        {
            
        }
    }
}
