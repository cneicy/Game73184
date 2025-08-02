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
        public Card NowCard { get; set; }
        
        private void Start()
        {
            state = BuiltState.Waiting;
        }

        private void Update()
        {
            switch (state)
            {
                case BuiltState.Waiting:
                    foreach (var slot in map.allSlots)
                    {
                        slot.isBuilding = false;
                    }
                    break;
                case BuiltState.Building:
                    foreach (var slot in map.allSlots)
                    {
                        slot.isBuilding = true;
                    }
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
    }
}
