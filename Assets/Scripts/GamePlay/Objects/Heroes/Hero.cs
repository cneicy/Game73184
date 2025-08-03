using System.Collections;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class Hero : MonoBehaviour, IInteractable
    {
        public static readonly int MaxHealth = 150000;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int health;
        [SerializeField] private DamageType damageType;
        [SerializeField] private float dRPer;
        
        public float DRPer{get=> dRPer;set => dRPer = value;}
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

        [EventSubscribe("NewGame")]
        public object OnNewGame(string a)
        {
            health = MaxHealth;
            return null;
        }

        [EventSubscribe("AttackHero")]
        public object OnGetHurt(TowerAttack towerAttack)
        {
            switch (towerAttack.DamageType)
            {
                case DamageType.AP:
                {
                    var finalDamage = towerAttack.Damage;
                    particles.Play();
                    Health -= finalDamage;
                    StartCoroutine(ColorChange());
                    EventManager.Instance.TriggerEvent("Harvest", finalDamage);
                    CheckHealth();
                    DamageType = DamageType.AP;
                    return finalDamage;
                }
                case DamageType.Clear:
                {
                    dRPer = 0;
                    var finalDamage = towerAttack.Damage;
                    particles.Play();
                    Health -= finalDamage;
                    StartCoroutine(ColorChange());
                    EventManager.Instance.TriggerEvent("Harvest", finalDamage);
                    CheckHealth();
                    DamageType = DamageType.Clear;
                    return finalDamage;
                }
                case DamageType.Normal:
                {
                    dRPer += health / (float)MaxHealth * 0.1f;
                    if (dRPer >= 1)
                    {
                        dRPer = 1;
                        return 0;
                    }

                    DamageType = DamageType.Normal;
                    if (towerAttack.Damage > 2.5f * (health / (float)MaxHealth))
                    {
                        var finalDamage = (int)((1 - dRPer) * towerAttack.Damage);
                        Debug.Log("最终伤害："+finalDamage);
                        particles.Play();
                        health -= finalDamage;
                        StartCoroutine(ColorChange());
                        EventManager.Instance.TriggerEvent("Harvest", finalDamage);
                        CheckHealth();
                        return finalDamage;
                    }
                    
                    particles.Play();
                    health -= towerAttack.Damage;
                    StartCoroutine(ColorChange());
                    EventManager.Instance.TriggerEvent("Harvest", towerAttack.Damage);
                    CheckHealth();
                    return towerAttack.Damage;
                }
                default:
                    return -1;
            }
        }

        public void CheckHealth()
        {
            if (!(health <= 0)) return;
            health = 0;
            Debug.Log("Hero死亡");
            EventManager.Instance.TriggerEvent("GameOver", "等待戈多");
            gameObject.SetActive(false);
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