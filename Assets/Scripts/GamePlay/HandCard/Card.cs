using GamePlay.Objects;
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

        public TowerType TowerType;
        public GameObject TowerPrefab;
        public bool CanBuildOnRoad;
        public Sprite CardFace;
        public Sprite CardBack;
        public float cd;
        //充能
        public float maxChargingIndex;
        public float nowChargingIndex;
        private float _maskProportion;
        
        private Coroutine _flipBackCoroutine;
        private void Start()
        {
            isFlipped = false;
            _isSelected = false;
            _cardLocalPosition = transform.localPosition;
            _localTransform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if(_spriteRenderer) _spriteRenderer.sprite = CardFace;
        }
        

        
        
        public void EndCooldownImmediately()
        {
            // 确保卡牌当前处于翻转状态（背面朝上）
            if (!isFlipped) return;

            // 停止正在进行的翻转协程
            if (_flipBackCoroutine != null)
            {
                StopCoroutine(_flipBackCoroutine);
                _flipBackCoroutine = null;
            }

            // 立即执行翻转
            CardEffect.Instance.CardFlip(isFlipped, transform);
            isFlipped = !isFlipped; // 更新翻转状态
        
            // 重置充能状态
            nowChargingIndex = maxChargingIndex;
        }
        
        public void UpdateCardData(Card cardData)
        {
            TowerType = cardData.TowerType;
            TowerPrefab = cardData.TowerPrefab;
            CanBuildOnRoad = cardData.CanBuildOnRoad;
            CardFace = cardData.CardFace;
            CardBack = cardData.CardBack;
            maxChargingIndex = cardData.maxChargingIndex;
            cd = cardData.cd;
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
        
        public void AfterBuilt(float cd)
        {
            CardEffect.Instance.CardFlip(isFlipped, transform);
            isFlipped = !isFlipped;
            
            if (_flipBackCoroutine != null)
            {
                StopCoroutine(_flipBackCoroutine);
            }
            _flipBackCoroutine = StartCoroutine(FlipBackAfterDelay(cd));
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
        
        private System.Collections.IEnumerator FlipBackAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
    
            
            CardEffect.Instance.CardFlip(isFlipped, transform);
            isFlipped = !isFlipped;
    
            _flipBackCoroutine = null;
        }
    }
}
