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
        public float PlaceCoolDown { get; set; }
        public int Damage { get; set; }
        public float AttackSpeed;
        public float AttackCoolDown;
        public int Cost { get; set; }

        public void Move()
        {
            
        }

        private void FixedUpdate()
        {
            Move();
        }

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