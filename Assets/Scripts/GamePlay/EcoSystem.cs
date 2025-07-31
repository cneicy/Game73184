using System.Collections.Generic;
using Event;
using GamePlay.Objects;
using Singleton;

namespace GamePlay
{
    public class EcoSystem : Singleton<EcoSystem>
    {
        public float Money { get; set; }
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

        [EventSubscribe("Harvest")]
        public object OnHarvest(float harvestMoney)
        {
            Money += harvestMoney;
            return harvestMoney;
        }

        private void Start()
        {
            TowerPriceIndex.Add(TowerType.UnDefine,0);
        }
    }
}