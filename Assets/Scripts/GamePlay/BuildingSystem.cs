using System;
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
        
        private void Start()
        {
            state = BuiltState.Waiting;
        }

        private void Update()
        {
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

        private void CancelBuilding()
        {
            if (state == BuiltState.Building)
            {
                if (UnityEngine.Input.GetMouseButtonDown(2))
                {
                    state = BuiltState.Waiting;
                }
            }
        }

        public void ChangeChargingIndex(int index)
        {
            if (NowCard.nowChargingIndex>=1)
            {
                NowCard.nowChargingIndex -= index;
            }
            else
            {
                //进入冷却，遮罩移除,卡牌翻面
            }
        }
    }
}
