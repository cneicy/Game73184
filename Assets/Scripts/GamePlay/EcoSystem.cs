using System.Collections.Generic;
using Event;
using GamePlay.Objects;
using Singleton;

namespace GamePlay
{
    public class EcoSystem : Singleton<EcoSystem>
    {
        public int Money { get; set; }
        public Dictionary<TowerType,int> TowerPriceIndex { get; set; }

        private void OnEnable()
        {
            if(EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if(EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }

        private void Start()
        {
            TowerPriceIndex.Add(TowerType.UnDefine,0);
        }
    }
}