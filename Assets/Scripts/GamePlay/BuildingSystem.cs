using System;
using System.Collections.Generic;
using GamePlay.HandCard;
using UnityEngine;
using Singleton;

namespace GamePlay
{
    public class BuildingSystem : Singleton<BuildingSystem>
    {
        public enum BuiltState
        {
            Waiting,
            Building,
        }

        public BuiltState state;
        [SerializeField] private Map map;
        public Card NowCard;
        public List<Card> handCards;
        
        private void Start()
        {
            state = BuiltState.Waiting;
            handCards = new List<Card>();
        }

        private void Update()
        {
            if (state == BuiltState.Building)
            {
                if (UnityEngine.Input.GetMouseButtonDown(2))
                {
                    ChangeStateToWaiting();
                }
            }
            
            switch (state)
            {
                case BuiltState.Waiting:
                    break;
                case BuiltState.Building:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        

        public void ChangeChargingIndex(int index)
        {
            if (NowCard.nowChargingIndex>1)
            {
                
            }
            else if (NowCard.nowChargingIndex==1)
            {
                //进入冷却，遮罩移除,卡牌翻面
                foreach (var card in handCards)
                {
                    if (card.TowerType==NowCard.TowerType)
                    {
                        card.AfterBuilt();
                    }
                }
                NowCard.nowChargingIndex -= index;
            }
            else
            {
                
            }
        }

        public void ChangeStateToBuilding()
        {
            state = BuiltState.Building;
        }

        public void ChangeStateToWaiting()
        {
            state = BuiltState.Waiting;
        }
    }
}
