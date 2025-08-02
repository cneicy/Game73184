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
        public DamageType DamageType { get; set; }
        public TowerType TowerType { get; set; }
        public float PlaceCoolDown { get; set; }
        public int Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float AttackCoolDown { get; set; }
        public int Cost { get; set; }

        protected abstract void Move();

        private void FixedUpdate()
        {
            Move();
        }

        public void Attack()
        {
            EventManager.Instance.TriggerEvent("AttackHero", new TowerAttack(DamageType, Damage));
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