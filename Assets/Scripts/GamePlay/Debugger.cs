using Event;
using GamePlay.Objects;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay
{
    public class Debugger : MonoBehaviour
    {
        public void HitBoss()
        {
            EventManager.Instance.TriggerEvent("AttackHero", new TowerAttack(DamageType.Normal, 1));
        }
        
    }
}