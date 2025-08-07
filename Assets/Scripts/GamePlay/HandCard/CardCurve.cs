using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Event;
using UnityEngine;
using UnityEngine.Splines;

namespace GamePlay.HandCard
{
    
    public class CardCurve : MonoBehaviour
    {
        [SerializeField] private int maxHandSize;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<Card> cardsList = new();
        private List<GameObject> _handCards = new();
        private Vector3 _difPos = new(-6,-12,0);
        private bool _isDrawing;
        private bool _isHiding;
        private bool show;

        [EventSubscribe("PowerOn")]
        public object StartCardAnimation(string s = "")
        {
            if(!show)
            {
                show = true;
                StartCoroutine(nameof(DrawAllCard));
            }
            
            return null;
        }

        [EventSubscribe("PowerOff")]
        public object HideCardAnimation(string s = "")
        {
            foreach (var card in _handCards)
            {
                card.GetComponent<Card>().EndCooldownImmediately();
            }
            return null;
        }
        
        private void OnEnable()
        {
            if (EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
        
        
        private IEnumerator DrawAllCard()
        {
            yield return new WaitForSeconds(0.01f);
            if(GameManager.Instance.GameState != GameState.Playing) yield break;
            print("开始动画协程");
            
            _isDrawing = true;
            
            int cardsToDraw = cardsList.Count;
            
            for (int i = 0; i < cardsToDraw; i++)
            {
                DrawCard();
                yield return new WaitForSeconds(0.25f); // 等待间隔
            }
            
            _isDrawing = false;
        }
        
        private void DrawCard()
        {
            if(_handCards.Count>=maxHandSize) return;
            GameObject g = Instantiate(cardPrefab,spawnPoint.localPosition,spawnPoint.localRotation);
            _handCards.Add(g);
            UpdateCardPositions();
        }
        
        private void UpdateCardPositions()
        {
            if (_handCards.Count == 0)  return;
            float cardSpacing = 1f / (maxHandSize+2);
            float firstCardPosition = 0.5f-(_handCards.Count-1)*cardSpacing/2;
            Spline spline = splineContainer.Spline;
            for (int i = 0; i < _handCards.Count; i++)
            {
                float p = firstCardPosition + i*cardSpacing;
                Vector3 splinePosition = spline.EvaluatePosition(p);
                Vector3 forward = spline.EvaluateTangent(p);
                Vector3 up = spline.EvaluateUpVector(p);
                Quaternion rotation = Quaternion.LookRotation(up,Vector3.Cross(up,forward).normalized);
                _handCards[i].transform.DOMove(splinePosition+_difPos, 0.25f);
                _handCards[i].transform.DORotateQuaternion(rotation, 0.25f);
                BuildingSystem.Instance.handCards.Add(_handCards[i].GetComponent<Card>());
            }
            UpdateCardData();
        }

        private void UpdateCardData()
        {
            for (int i = 0; i < _handCards.Count; i++)
            {
                _handCards[i].GetComponent<Card>().UpdateCardData(cardsList[i]);
            }
        }
        
        // 新增的HideAllCard方法
        private IEnumerator HideAllCard()
        {
            // 如果没有卡牌或正在隐藏，直接返回
            if (_handCards.Count == 0 || _isHiding)
            {
                yield break;
            }
            
            _isHiding = true;
            print("开始隐藏卡牌");
            
            // 创建DOTween序列
            Sequence hideSequence = DOTween.Sequence();
            
            // 为每张卡牌创建动画
            for (int i = _handCards.Count - 1; i >= 0; i--)
            {
                GameObject card = _handCards[i];
                
                // 创建卡牌的动画序列
                Sequence cardSequence = DOTween.Sequence();
                
                // 将卡牌移动回生成点
                cardSequence.Append(card.transform.DOMove(spawnPoint.position, 0.5f)
                    .SetEase(Ease.InBack));
                
                // 旋转回原始角度
                cardSequence.Join(card.transform.DORotateQuaternion(spawnPoint.rotation, 0.5f)
                    .SetEase(Ease.InBack));
                
                // 缩小卡牌
                cardSequence.Append(card.transform.DOScale(Vector3.zero, 0.3f)
                    .SetEase(Ease.InBack));
                
                // 在动画完成后销毁卡牌
                cardSequence.OnComplete(() => {
                    // 从BuildingSystem中移除卡牌
                    if (BuildingSystem.Instance != null && card.GetComponent<Card>() != null)
                    {
                        BuildingSystem.Instance.handCards.Remove(card.GetComponent<Card>());
                    }
                    
                    // 销毁卡牌对象
                    Destroy(card);
                });
                
                // 将卡牌动画序列添加到主序列中，使用Join使所有卡牌同时动画
                hideSequence.Join(cardSequence);
            }
            
            // 等待所有动画完成
            yield return hideSequence.WaitForCompletion();
            
            // 清空手牌列表
            _handCards.Clear();
            
            _isHiding = false;
            print("所有卡牌已隐藏");
        }
    }
}