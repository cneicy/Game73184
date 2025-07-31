using Event;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class Hero : MonoBehaviour
    {
        public int health;
        
        public int Health
        {
            get => health;
            set => health = value;
        }

        [EventSubscribe("AttackHero")]
        public object OnGetHurt(int damage)
        {
            Health -= damage;
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