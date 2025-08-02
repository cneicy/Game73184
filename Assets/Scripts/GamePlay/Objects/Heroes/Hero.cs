using System.Collections;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class Hero : MonoBehaviour, IInteractable
    {
        public static readonly int MaxHealth = 200;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int health;
        [SerializeField] private DamageType damageType;
        [SerializeField] private float dRPer;
        public DamageType DamageType
        {
            get => damageType;
            set => damageType = value;
        }

        public int Health
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
            health = MaxHealth;
            spriteRenderer = GetComponent<SpriteRenderer>();
            Clear();
        }

        public void Interact()
        {
            Debug.Log("Interact");
        }

        public object Clear()
        {
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
            if (towerAttack.DamageType == DamageType.AP)
            {
                var finalDamage = towerAttack.Damage;
                particles.Play();
                Health -= finalDamage;
                StartCoroutine(ColorChange());
                EventManager.Instance.TriggerEvent("Harvest", finalDamage);
                CheckHealth();
                return finalDamage;
            }

            if (towerAttack.DamageType == DamageType.Clear)
            {
                dRPer = 0;
                var finalDamage = towerAttack.Damage;
                particles.Play();
                Health -= finalDamage;
                StartCoroutine(ColorChange());
                EventManager.Instance.TriggerEvent("Harvest", finalDamage);
                CheckHealth();
                return finalDamage;
            }

            if(towerAttack.DamageType == DamageType.Normal)
            {
                if (dRPer == 0)
                {
                    dRPer += towerAttack.Damage/(float)MaxHealth * 0.1f;
                    health -= towerAttack.Damage;
                    EventManager.Instance.TriggerEvent("Harvest", towerAttack.Damage);
                    return towerAttack.Damage;
                }
                else
                {
                    dRPer += towerAttack.Damage/(float)MaxHealth *dRPer * 0.1f;
                }

                if (towerAttack.Damage == 0)
                {
                    health -= towerAttack.Damage;
                    EventManager.Instance.TriggerEvent("Harvest", towerAttack.Damage);
                    return towerAttack.Damage;
                }
            }

            return MaxHealth;
        }

        public void CheckHealth()
        {
            if (!(health <= 0)) return;
            health = 0;
            Debug.Log("Hero死亡");
            EventManager.Instance.TriggerEvent("HeroDead", "等待戈多");
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