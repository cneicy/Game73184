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

        [EventSubscribe("Harvest")]
        public object OnHarvest(int harvestMoney)
        {
            Money += harvestMoney;
            return harvestMoney;
        }

        private void Start()
        {
            TowerPriceIndex = new Dictionary<TowerType,int>
            {
                { TowerType.UnDefine, 0 },
                { TowerType.Domino, 10 },
                { TowerType.BlindEye, 11 },
                { TowerType.CompetitiveFulcrum, 12 },
                { TowerType.Mirror, 13 }
            };
        }
    }
}