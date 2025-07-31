using System;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class Hero : MonoBehaviour
    {
        public float health;
        public DamageType damageType;
        public int damageTime;
        public float Health
        {
            get => health;
            set => health = value;
        }

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
            Clear();
        }

        public object Clear()
        {
            damageTime = 0;
            damageType = DamageType.UnDefine;
            return null;
        }

        [EventSubscribe("AttackHero")]
        public object OnGetHurt(TowerAttack towerAttack)
        {
            if (towerAttack.DamageType != damageType)
            {
                damageTime = 0;
                damageType = towerAttack.DamageType;
            }
            else damageTime++;
            var finalDamage = towerAttack.Damage - damageTime * 0.1f;
            Health -= finalDamage;
            EventManager.Instance.TriggerEvent("Harvest", finalDamage);
            return finalDamage;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Tower"))
            {
                //todo:等待戈多
                EventManager.Instance.TriggerEvent("AttackTower", "此处等待策划");
            }
        }
    }
}