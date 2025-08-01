using System.Collections.Generic;
using Event;
using GamePlay.Objects;
using Singleton;
using UnityEngine;

namespace GamePlay
{
    public class EcoSystem : Singleton<EcoSystem>
    {
<<<<<<< HEAD
        public float Money { get; set; }
        public Dictionary<TowerType,int> TowerPriceIndex { get; set; }
=======
        public int Money { get; set; }
        public Dictionary<TowerType,int> TowerPriceIndex { get; set; } = new Dictionary<TowerType,int>();
>>>>>>> cc9cb3d6733d859d9ba72e4967088586ac034ff4

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
            Debug.Log("NowMoney:"+Money);
            return harvestMoney;
        }

        private void Start()
        {
            TowerPriceIndex = new Dictionary<TowerType,int>
            {
                { TowerType.UnDefine, 0 },
                { TowerType.Domino, 10 },
                { TowerType.Blindeye, 11 },
                { TowerType.CompetitiveFulcrum, 12 },
                { TowerType.Mirror, 13 }
            };
        }
    }
}