using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Objects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace GamePlay.HandCard
{
    
    public class CardCurve : MonoBehaviour
    {
        [SerializeField] private int maxHandSize;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<Card> cardsList = new List<Card>();
        private List<GameObject> _handCards = new List<GameObject>();
        private Vector3 _difPos = new Vector3(-6,-12,0);
        private bool _isDrawing = false;


        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                StartCardAnimation();
            }
        }

        public void StartCardAnimation()
        {
            print("卡牌动画");
            StartCoroutine(DrawAllCard());
        }
        
        private IEnumerator DrawAllCard()
        {
            print("开始动画协程");
            
            _isDrawing = true;
            
            // 计算需要补充的卡牌数量
            int cardsToDraw = 5;
            
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
        
    }
}
