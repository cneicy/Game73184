using Event;
using GamePlay.Objects;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay
{
    public class Debugger : MonoBehaviour
    {
        [SerializeField] private DamageType damageType;
        [SerializeField] private int damageAmount;
        public void HitBoss()
        {
            EventManager.Instance.TriggerEvent("AttackHero", new TowerAttack(damageType, damageAmount));
        }
        
    }
}