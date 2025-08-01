using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
    public class BuildingSystem : MonoBehaviour
    {
        public enum BuiltState
        {
            Waiting,
            Building,
        }

        public BuiltState state = BuiltState.Waiting;
        [SerializeField] private Map map;
        
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
                        slot._isBuilding = false;
                    }
                    break;
                case BuiltState.Building:
                    foreach (var slot in map.allSlots)
                    {
                        slot._isBuilding = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
