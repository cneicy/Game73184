using System.Collections.Generic;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class Hero : MonoBehaviour
    {
        public int health;
        public Dictionary<DamageType, int> DamageSourceTimes;
        public int Health
        {
            get => health;
            set => health = value;
        }

        private void Start()
        {
            DamageSourceTimes = new Dictionary<DamageType, int> { { DamageType.Normal, 0 } };
            Clear();
        }

        public object Clear()
        {
            DamageSourceTimes[DamageType.Normal] = 0;
            return null;
        }

        [EventSubscribe("AttackHero")]
        public object OnGetHurt(TowerAttack towerAttack)
        {
            Health -= towerAttack.Damage;
            DamageSourceTimes[towerAttack.DamageType]++;
            return Health;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //todo : 被“子弹”攻击
            /*if(other.CompareTag("此处写攻击物"))
                EventManager.Instance.TriggerEvent("HeroGetHurt",health);*/
        }
    }
}