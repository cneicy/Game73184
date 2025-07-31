using System.Collections.Generic;
using GamePlay.Objects.Towers.Strategy;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public abstract class AbstractTower : MonoBehaviour
    {
        public List<BaseTowerStrategy> Upgrades { get; set; } = new();
        public float CoolDown { get; set; }
        public int Damage { get; set; }

        public void Attack()
        {
            foreach (var upgrade in Upgrades)
            {
                if (upgrade is ITowerStrategy towerStrategy)
                {
                    towerStrategy.ApplyUpgrade(this);
                }
            }
        }
    }
}