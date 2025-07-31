using System.Collections.Generic;
using Event;
using GamePlay.Objects.Towers.Strategy;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public struct TowerAttack
    {
        public readonly DamageType DamageType;
        public readonly int Damage;

        public TowerAttack(DamageType damageType, int damage)
        {
            DamageType = damageType;
            Damage = damage;
        }
    }
    public abstract class AbstractTower : MonoBehaviour
    {
        public List<BaseTowerStrategy> Upgrades { get; set; } = new();
        public DamageType damageType;
        public float CoolDown { get; set; }
        public int Damage { get; set; }

        public void Attack()
        {
            EventManager.Instance.TriggerEvent("AttackHero", new TowerAttack(damageType, Damage));
        }
        public void Upgrade()
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