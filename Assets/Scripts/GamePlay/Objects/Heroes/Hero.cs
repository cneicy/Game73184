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
            return finalDamage;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //todo : 被“子弹”攻击
            /*if(other.CompareTag("此处写攻击物"))
                EventManager.Instance.TriggerEvent("HeroGetHurt",health);*/
        }
    }
}