using System.Collections;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class Hero : MonoBehaviour , IInteractable
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField]private float health;
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
            spriteRenderer = GetComponent<SpriteRenderer>();
            Clear();
        }

        public void Interact()
        {
            Debug.Log("Interact");
        }

        public object Clear()
        {
            damageTime = 0;
            damageType = DamageType.UnDefine;
            return null;
        }

        private IEnumerator ColorChange()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }

        [EventSubscribe("AttackHero")]
        public object OnGetHurt(TowerAttack towerAttack)
        {
            particles.Play();
            if (towerAttack.DamageType != damageType)
            {
                damageTime = 0;
                damageType = towerAttack.DamageType;
            }
            else damageTime++;
            var finalDamage = towerAttack.Damage - damageTime * 0.1f;
            Health -= finalDamage;
            StartCoroutine(ColorChange());
            EventManager.Instance.TriggerEvent("Harvest", finalDamage);
            CheckHealth();
            Debug.Log("Hero Hit:"+finalDamage+"HPL:"+health);
            return finalDamage;
        }

        public void CheckHealth()
        {
            if (health <= 0)
            {
                EventManager.Instance.TriggerEvent("HeroDead","等待戈多");
            }
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