using System;
using System.Collections.Generic;
using DG.Tweening;
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
        private List<GameObject> _handCards = new List<GameObject>();
        private Vector3 _difPos = new Vector3(-6,-6,0);

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.A))
            {
                DrawCard();
            }
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
            float cardSpacing = 1f / maxHandSize;
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
            }
        }
    }
}
