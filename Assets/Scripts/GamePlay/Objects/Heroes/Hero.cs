using System.Collections;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class Hero : MonoBehaviour, IInteractable
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float health;
        [SerializeField] private DamageType damageType;
        [SerializeField] private int damageTime;
        
        public DamageType DamageType
        {
            get => damageType;
            set => damageType = value;
        }

        public int DamageTime
        {
            get => damageTime;
            set => damageTime = value;
        }

        public float Health
        {
            get => health;
            set => health = value;
        }

        private void OnEnable()
        {
            if (EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
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
            DamageTime = 0;
            DamageType = DamageType.UnDefine;
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
            if (towerAttack.DamageType != DamageType)
            {
                DamageTime = 0;
                DamageType = towerAttack.DamageType;
            }
            else DamageTime++;

            var finalDamage = towerAttack.Damage - DamageTime * 0.1f;
            if(finalDamage <= 0) return finalDamage;
            particles.Play();
            Health -= finalDamage;
            StartCoroutine(ColorChange());
            EventManager.Instance.TriggerEvent("Harvest", finalDamage);
            CheckHealth();
            Debug.Log("Hero Hit:" + finalDamage + "HPL:" + health);
            return finalDamage;
        }

        public void CheckHealth()
        {
            if (health <= 0)
            {
                EventManager.Instance.TriggerEvent("HeroDead", "等待戈多");
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